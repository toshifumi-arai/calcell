//===================================================================
// 電卓もどきと筆算実行プログラム
// © 2016 Toshifumi Arai (toshifumi@acm.org)
//===================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace CalCell
{
    /// <summary>
    /// 筆算の終了時の状態
    /// </summary>
    enum CellStat { Ok, Abort, DivByZero, OutOfRange, NegativeValue };

    /// <summary>
    /// 升目を使った筆算を実行するクラス
    /// </summary>
    class CellField
    {
        // 描画用pictureBoxとGraphics
        private PictureBox pictureBox = null;   // 描画領域。実際には、これに付けてあるBitmapに描画する。
                                                // Bitmapの方に描画しておけば、InvalidateでUIスレッドに再描画を要求した
                                                // とき、そのBitmapの内容を勝手に再描画してもらえる。
        private Graphics g;                     // pictureBoxに付けたBitmapに描画するためのグラフィクス

        // 描画に関わる定数
        private const int cellWidth   = 24;                                     // 枡一つの幅
        private const int cellHeight  = 32;                                     // 枡一つの高さ
        private const int leftMargin  = 4,  rightMargin = 4,  xMargin = 10;     // 横方向のマージン
        private const int upperMargin = 4,  lowerMargin = 4,  yMargin = 10;     // 縦方向のマージン
        private const int cellPitchX  = cellWidth + xMargin;                    // 横方向の枡の間隔
        private const int cellPitchY  = cellHeight + yMargin;                   // 縦方向の枡の間隔

        // 升目を表示する色やフォントなど
        private Color   baseColor     = Color.WhiteSmoke;                       // 地の色
        private Brush   numBrush      = Brushes.Black;                          // 枡の中の数字を書く色
        private Brush   numBrush2     = Brushes.DarkGray;                       // 枡の左肩の小さい数字を書く色
        private Font    numFont       = new Font("Arial", 18, FontStyle.Bold);  // 枡の中の数字を書くフォント
        private Font    numFont2      = new Font("Arial", 10, FontStyle.Bold);  // 枡の左肩の小さい数字を書くフォント
        private Pen     underLinePen  = new Pen(Color.Black, 4);                // 枡の下線を描くペンの色と太さ

        // 升目の横方向、縦方向の個数
        private int numX = 40, numY = 50;       // 筆算に使う升目の個数の望ましい数値。
                                                // 実際には、コンストラクタに渡された描画領域のサイズで調整される。
                                                //
                                                // 40×50の根拠は下記：
                                                // c#のlongは、-9,223,372,036,854,775,808 ～ 9,223,372,036,854,775,807
                                                // 最大で19桁の数。19桁の数どうしを掛け算すると、答えは38桁になる。
                                                // (もちろん、longには収まらないので、桁あふれエラーになるが)
                                                // また、割り算を書く時の横幅も19+19+1(演算子)で、39が最大となる。
                                                // したがって、筆算に使う升目の幅は40で十分である。
                                                // 一方で、縦の長さを考えると、最大になりそうなのは、19桁の数を1桁の数で
                                                // 割り算する場合であろう。1桁進むのに2行使うので、最初の2行に加えて、
                                                // 19×2＝38必要。合計で40となる。少々余裕を持たせて、50にしておく。
                                                //
                                                // 今は、内部で持つデータの縦横と、表示できる升目の縦横は同じにしている。
                                                // 本当は分けたほうが良いのだが、ひとまず時間切れで...

        // 升目のデータ
        private CellMap cells = null;           // 各升目の値などを保持するデータ

        // 状態保持用
        private CellStat stat = CellStat.Ok;    // 終了時の状態
        private bool ABORT_FLAG = false;        // 処理を中止するためのフラグ

        // 大域脱出の例外識別用
        private static readonly String ABORT_MESSAGE = "ABORT";
        private static readonly String NEGVAL_MESSAGE = "NEGVAL";

        // 筆算の速度調整用
        private int waitControlValue;           // 速度調整用の値 0～100 の間で、0側が速い。0ならwait無し
        private int W = 10;                     // UpdateWithDelayのdelayは、msec * waitControlValue / W で算出

        public CellField(PictureBox pb)
        {
            pictureBox = pb;

            int fieldWidth = pictureBox.Width;
            int fieldHeight = pictureBox.Height;
            pictureBox.Image = new Bitmap(fieldWidth, fieldHeight);     // 描画用のビットマップを生成して登録
            g = Graphics.FromImage(pictureBox.Image);

            int X = (fieldWidth - leftMargin - rightMargin) / cellPitchX;
            int Y = (fieldHeight - upperMargin - lowerMargin) / cellPitchY;

            Console.WriteLine("枡の縦横は {0} x {1} になっています。", X, Y);
            if (X < numX || Y < numY) Console.WriteLine("枡の縦横は {0} x {1} 以上が望ましいですよ！", numX, numY);
            // こんな表示は誰も見ないと思うが、自分でチェックするために...

            numX = X;
            numY = Y;
        }

        /// <summary>
        /// 升目領域の表示をアップデートする(例外処理)
        /// </summary>
        /// <param name="ope"></param>
        /// <param name="waiting"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="go">筆算を実行するかどうかのフラグ</param>
        public void Update(OpeType ope, bool waiting, long value1, long value2, bool go)
        {
            try
            {
                UpdateBody(ope, waiting, value1, value2, go);
            }
            catch (System.IndexOutOfRangeException ex)          // 升目が足りないとき
            {
                // 今は、升目の縦横が十分に多いので、理屈上ここには来ない(上述のnumX,numYのコメント参照)。
                // 試すには、CellFieldPictureBoxのHeight(今は2200)を、300にして大きめの掛け算をすれば良い。
                stat = CellStat.OutOfRange;
                return;
            }
            catch (System.DivideByZeroException ex)             // ゼロ除算のとき
            {
                stat = CellStat.DivByZero;
                return;
            }
            catch (System.Exception ex)
            {
                if (ex.Message == ABORT_MESSAGE)                // ABORTボタンが押されたとき
                {
                    stat = CellStat.Abort;
                    return;
                }
                if (ex.Message == NEGVAL_MESSAGE)               // 負の値が出てきたとき
                {
                    stat = CellStat.NegativeValue;
                    return;
                }
                throw;                                          // 知らない例外は、さらに上に投げる。
            }
            stat = CellStat.Ok;                                 // 筆算が正常に終わったとき
        }

        /// <summary>
        /// 升目領域の表示をアップデートする(本体)
        /// </summary>
        /// <param name="ope"></param>
        /// <param name="waiting"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="go">筆算を実行するかどうかの指示</param>
        private void UpdateBody(OpeType ope, bool waiting, long value1, long value2, bool go)
        {
            cells = new CellMap(numX, numY);                    // 富豪的に毎回確保

            g.Clear(baseColor);                                 // 表示も最初からやり直す
            DrawCellFrames();

            if (ope == OpeType.Null && waiting)                 // 何も入力されていない状態
            {                                                   // 何も書かない
                // nothing to do
            }
            if (ope == OpeType.Null && !waiting)                // 最初の数の入力中
            {                                                   // 最初の数を右詰で書く
                PutNumber(value1, 1, 0);
            }
            if (ope != OpeType.Null && waiting)                 // 2つ目の数の最初の文字待ち
            {                                                   // 最初の数と演算子を右詰で書く
                PutNumber(value1, 1, 0);
                if (ope == OpeType.Div)                         // 割り算の時は、一つ目の数字の左側に演算記号を置く
                    PutOperationMark(ope, 1, NumLength(value1));
                else                                            // それ以外の時は、演算記号は次の行に置く
                    PutOperationMark(ope, 2, NumLength(value1));
            }
            if (ope != OpeType.Null && !waiting)                // 2つ目の数の入力中
            {                                                   // 最初の数、演算子、2つ目の数を右詰で書く
                PutNumber(value2, 1, 0);
                int len1 = NumLength(value1);
                int len2 = NumLength(value2);
                if (ope == OpeType.Div)                         // 割り算の時だけ、演算子と数字を各場所が違う
                {
                    PutOperationMark(ope, 1, len2);             // 演算記号は一つ目の数字の左
                    PutNumber(value1, 1, len2 + 1);             // 二つ目の数字は演算記号の左
                    if (go)
                    {
                        PutLine(0, 0, len2);
                        DoDiv(1, 0, len1, len2, value1, value2);
                    }
                }
                else
                {
                    int len = (len1 > len2) ? len1 : len2;      // 演算記号を置く位置は、桁数が多い方で決まる
                    PutOperationMark(ope, 2, len);              // 桁数が多い方より左に演算記号を置く
                    PutNumber(value1, 2, 0);                    // 2つ目の数字を置く
                    if (go)
                    {
                        PutLine(2, 0, len + 1);
                        if (ope == OpeType.Plus)  DoPlus(1, 0, len, 2, value1, value2);
                        if (ope == OpeType.Minus) DoMinus(1, 0, len, value1, value2, false);
                        if (ope == OpeType.Mul)   DoMul(1, 0, len, value1, value2);
                    }
                }
            }
            DrawCells();                                        // 升目をBitmapに描画し
            pictureBox.Invalidate();                            // UIスレッドに描画更新を依頼
        }

        /// <summary>
        /// UIスレッドからの、Update中止指示を受け付ける
        /// </summary>
        public void Abort()
        {
            ABORT_FLAG = true;
        }

        /// <summary>
        /// 筆算終了時の状態を返す
        /// </summary>
        /// <returns>筆算終了時の状態</returns>
        public CellStat GetStat()
        {
            return stat;
        }

        /// <summary>
        /// 足し算を筆算で。複数行の足し算も受け付ける(掛け算で使うので)。ただし... (remarks参照)
        /// </summary>
        /// <param name="lineNo">一番上の行の位置(0～)</param>
        /// <param name="fromRight">右端から何番目か(0～)</param>
        /// <param name="width">足し算の対象となる領域の幅</param>
        /// <param name="height">足し算の対象となる領域の高さ(何個の数を足し算するか)</param>
        /// <param name="value1">下側の数(必ず正の数)</param>
        /// <param name="value2">上側の数(負になっていることも)</param>
        /// <remarks>
        /// 複数行の足し算は、掛け算の筆算に使うことだけ考えている。
        /// このメソッドは、それで使われないパターンでは正常に動かないこともある。
        /// たとえば、1を100回足して100になる場合などは動かないであろう。たぶん "00" だけが表示される。
        /// (このプログラムでは、そういうケースは生じないが)
        /// 一方で、掛け算の筆算の場合を考えると、桁数aと桁数bの数を掛けた場合、答えは高々a+b桁である。
        /// したがって、このメソッドでは a+b桁までしか扱わないことにしている。
        /// </remarks>
        private void DoPlus(int lineNo, int fromRight, int width, int height, long value1, long value2)
        {
            if (value2 < 0)                                     // 上が負の数なら...
            {
                throw new Exception(NEGVAL_MESSAGE);
            }

            int iy = lineNo + height;                           // 答えを書く行の位置
            foreach (int i in Enumerable.Range(0, width))
            {
                int ix = numX - fromRight - i - 1;              // 対象となる桁の位置

                int sum = 0;
                foreach (int line in Enumerable.Range(lineNo, height))
                {
                    Cell c = cells.GetCellAt(ix, line);
                    int v = ValueOf(c);
                    sum += v;
                }
                Cell cRight = cells.GetCellAt(ix + 1, iy);
                sum += Value2Of(cRight);                        // 右からの繰り上がりも足す

                if (i == width - 1 && sum == 0) break;          // 最大桁が0なら描かない。汚い...

                cells.SetValue(ix, iy, sum % 10);
                UpdateWithDelay(200);
                cells.SetValue2(ix, iy, sum / 10);
                UpdateWithDelay(200);
            }

            // 答えの一番左の桁に繰り上がりがあるかチェック
            Cell cLast = cells.GetCellAt(numX - fromRight - width, iy);
            int vLast = Value2Of(cLast);
            if (0 < vLast)
            {
                cells.SetValue(numX - width - 1, iy, vLast);
                UpdateWithDelay(200);
            }
        }

        /// <summary>
        /// 引き算を筆算で。扱うのは、a - b という2つの数字の場合のみ。
        /// </summary>
        /// <param name="lineNo">2つの数字がある領域の最初(上側)の行指定(0～)</param>
        /// <param name="fromRight">2つの数字がある領域の右端が、升目の右端から何番目か(0～)</param>
        /// <param name="width">2つの数字がある領域の幅</param>
        /// <param name="value1">下側の数字(必ず正の数)</param>
        /// <param name="value2">上側の数字(負の数のこともある)</param>
        private void DoMinus(int lineNo, int fromRight, int width, long value1, long value2, bool blank)
        {
            int iy = lineNo + 2;                                // 答えを書く行の位置

            if (value2 < 0 || value2 < value1)                  // 上が負、または結果が負なら...
            {
                throw new Exception(NEGVAL_MESSAGE);
            }

            foreach (int i in Enumerable.Range(0, width))
            {
                int ix = numX - fromRight - i - 1;              // 対象となる桁の位置

                Cell cUpper = cells.GetCellAt(ix, lineNo);
                Cell cRight = cells.GetCellAt(ix + 1, lineNo);
                int vUpper = ValueOf(cUpper);
                if (Value2Of(cRight) > 0) vUpper -= 1;          // 右に1貸してた！

                Cell cLower = cells.GetCellAt(ix, lineNo + 1);
                int vLower = ValueOf(cLower);

                if (vUpper < vLower)                            // 左から借りてこなくちゃ！
                {
                    vUpper += 10;
                    cUpper.Value2 = 1;                          // 左から借りてきた印として
                    UpdateWithDelay(200);
                }
                int v = vUpper - vLower;

                cells.SetValue(ix, iy, v);
                UpdateWithDelay(200);
            }
            int w = (blank) ? width : width - 1;                // ブランクが許されない場合、最後の桁は残す
            foreach (int i in Enumerable.Range(0, w))           // 上位桁に余分な0があったら消しとく
            {
                int ix = numX - fromRight - width + i;
                Cell c = cells.GetCellAt(ix, iy);
                if (ValueOf(c) != 0) break;
                cells.SetValue(ix, iy, -1);
                g.Clear(baseColor);
                DrawCellFrames();
                UpdateWithDelay(200);
            }
        }

        /// <summary>
        /// 掛け算を筆算で
        /// </summary>
        /// <param name="lineNo">掛け算の上の数字がある場所の行指定(0～)</param>
        /// <param name="fromRight">数の領域の右端が、升目の右端から何番目か(0～)</param>
        /// <param name="width">数の領域の幅</param>
        /// <param name="value1">下側の数字(必ず正の数)</param>
        /// <param name="value2">上側の数字(負の数のこともある)</param>
        private void DoMul(int lineNo, int fromRight, int width, long value1, long value2)
        {
            if (value2 < 0)                                     // 上が負の数なら...
            {
                throw new Exception(NEGVAL_MESSAGE);
            }

            int len1 = CheckLength(lineNo, fromRight, width);
            int len2 = CheckLength(lineNo + 1, fromRight, width);
            int skipped = 0;                                    // 掛ける数が0なら表示をスキップ。その回数を数える。

            // 桁ごとに掛け算して
            foreach (int i in Enumerable.Range(0, len2))
            {
                int ix = numX - fromRight - i - 1;              // 対象となる桁の位置
                Cell c = cells.GetCellAt(ix, lineNo + 1);
                int n = ValueOf(c);
                if (len2 != 1 && n == 0)                        // 掛ける数が0なら表示はスキップ。ただし0単独ならスキップしない
                {
                    CopyUpperNum(0, lineNo + 2 + i, fromRight + i);
                    skipped++;
                    continue;
                }
                DoMulOne(n, lineNo, fromRight, len1, lineNo + 2 + i - skipped, fromRight + i);
            }

            if (len2 - skipped == 1) return;                    // 掛け算が一回なら、結果の合計(以下の処理)は必要なし。

            // 線を引いて
            foreach (int i in Enumerable.Range(0, len1 + len2))
            {
                cells.SetUnderline(numX - fromRight - i - 1, lineNo + 2 + len2 - skipped - 1, true);
                // 線を引く行は、以下のように計算。備忘録。
                // lineNo +2(value2とvalue1) + len2(掛ける数の桁数) - skipped(0乗算を書かなかった数) - 1(その上の行)
            }

            // 結果を合計する
            DoPlus(lineNo + 2, fromRight, len1 + len2, len2 - skipped, 1, 1);
            // 最後の1,1は、上側が負の数ではないという意味。
        }

        /// <summary>
        /// 一桁の数と、(複数桁の)数を掛け算する。答えは指定された場所に書く
        /// </summary>
        /// <param name="n">一桁の数</param>
        /// <param name="lineNo">掛け算の対象となる数が描かれた場所の行指定(0～)</param>
        /// <param name="fromRight">掛け算の対象となる数が描かれた場所の右端からの位置指定(0～)</param>
        /// <param name="width">掛け算の対象となる数の幅(桁数)</param>
        /// <param name="lineNoA">答えを書く場所の行指定(0～)</param>
        /// <param name="fromRightA">答えを各場所の右端からの位置指定(0～)</param>
        private void DoMulOne(int n, int lineNo, int fromRight, int width, int lineNoA, int fromRightA)
        {
            int len = (n == 0) ? 1 : CheckLength(lineNo, fromRight, width); // 0を掛けた答えは0を1個だけ書く
            foreach (int i in Enumerable.Range(0, len))
            {
                int ix = numX - fromRight - i - 1;
                int ixA = numX - fromRightA - i - 1;
                Cell cValue = cells.GetCellAt(ix, lineNo);
                Cell cRight = cells.GetCellAt(ixA + 1, lineNoA);
                int d = ValueOf(cValue) * n + Value2Of(cRight);             // Value2Of(cRight) は右からの繰り上がり
                cells.SetValue(ixA, lineNoA, d % 10);
                UpdateWithDelay(100);
                cells.SetValue2(ixA, lineNoA, d / 10);
                UpdateWithDelay(100);
            }
            // 答えの一番左の桁に繰り上がりがあるかチェック
            Cell cLast = cells.GetCellAt(numX - fromRightA - len, lineNoA);
            int vLast = Value2Of(cLast);
            if (0 < vLast)
            {
                cells.SetValue(numX - fromRightA - len - 1, lineNoA, vLast);
                UpdateWithDelay(200);
            }
        }

        /// <summary>
        /// 割り算を筆算で
        /// </summary>
        /// <param name="lineNo">式が書いてある場所の行指定(0～)</param>
        /// <param name="fromRight">式の右端は升目の右端から何番目か(0～)</param>
        /// <param name="width1">割る数の幅(桁数)</param>
        /// <param name="width2">割られる数の幅(桁数)</param>
        /// <param name="value1">割る数</param>
        /// <param name="value2">割られる数</param>
        /// <remarks>
        /// value2(割られる数)の場所は、lineNo行目、右端からfromRight、幅width2
        /// value1(割る数)の場所は、    lineNo行目、右端からfromRight + width2 + 1、幅width1
        /// </remarks>
        private void DoDiv(int lineNo, int fromRight, int width1, int width2, long value1, long value2)
        {
            if (value2 < 0)                                 // 上が負の数なら...
            {
                throw new Exception(NEGVAL_MESSAGE);
            }

            long ans = value2 / value1;                     // いきなりズルしているようで少々気が引けるが...

            int len1 = NumLength(value1);
            int len2 = NumLength(value2);
            int lenAns = NumLength(ans);

            int skipped = 0;                                // 答えのある桁が0だったら、そこはスキップするので、何回かを覚える

            foreach (int i in Enumerable.Range(0, lenAns))  // 答えを左から置いていく
            {
                int d = lenAns - i - 1;                     // 上位からi(0～)番目の桁
                int n = DigitNum(ans, d);                   // 上位からi(0～)番目の桁にある値
                int fr = fromRight + d;                     // その桁は升目の右端から何番目か
                PutNumber(n, lineNo - 1, fr);               // その桁を升目上に置く

                int ln = lineNo + (2 * (i - skipped));      // 注目している行の番号
                int w = CheckLength(ln, fr, len2);          // 注目している行の部分に何桁の数字があるか

                if (n == 0)                                 // 答えの中で0の桁は掛け算と引き算をスキップ
                {
                    skipped++;
                    if (i != lenAns - 1)
                    {
                        PutLine(ln - 1, fr - 1, 1);
                        CopyUpperNum(ValueOf(cells.GetCellAt(numX - fr, lineNo)), ln, fr - 1);
                    }
                }
                else
                {
                    DoMulOne(n, 1, len2 + 1, len1, ln + 1, fr);
                    PutLine(ln + 1, d, w);                  // 線を引く

                    if (i != lenAns - 1)                    // 最後の桁でなければ...
                    {
                        DoMinus(ln, fr, w, 1, 2, true);
                        // 後ろの方の1,2は「引き算の上下を入れ替えなくて良い」という意味
                        // 最後のtrueは「引き算の結果が0の場合、表示はブランクで良い」という意味
                        PutLine(ln + 1, fr - 1, 1);
                        CopyUpperNum(ValueOf(cells.GetCellAt(numX - fr, lineNo)), ln + 2, fr - 1);
                    }
                    else                                    // 最後の桁なら...
                    {
                        DoMinus(ln, fr, w, 1, 2, false);
                        // 後ろの方の1,2は「引き算の上下を入れ替えなくて良い」という意味で入れている
                        // 最後のfalseは「引き算の結果が0の場合、0を一つ表示する」という意味
                    }
                }
            }
        }

        /// <summary>
        /// 割り算の筆算で、上の数字を降ろしてくる操作
        /// </summary>
        /// <param name="v">降ろしてくる数(0～9)</param>
        /// <param name="lineNo">降ろす先の行指定(0～)</param>
        /// <param name="fromRight">降ろす先の位置が升目の右端から何番目か(0～)</param>
        private void CopyUpperNum(int v, int lineNo, int fromRight)
        {
            if (v != 0 || fromRight == 0)                   // 0でないか、最後の桁なら、必ず降ろしてくる
            {                                               // "fromRight == 0" は要注意... 今は動くけど。
                PutNumber(v, lineNo, fromRight);
                return;
            }

            Cell cLeft = cells.GetCellAt(numX - fromRight - 2, lineNo);
            if (cLeft != null && cLeft.Value >= 0)          // 左側に有効な数字があれば降ろしてくる
            {
                PutNumber(v, lineNo, fromRight);
                return;
            }

            // 上記以外の場合には、上の数字を降ろしてこない。
        }

        /// <summary>
        /// 枡のValueの値(0～9)を取得
        /// </summary>
        /// <param name="c">枡のデータ</param>
        /// <returns>枡のValueの値(0～9)</returns>
        /// <remarks>
        /// 枡がnullの場合は0を返す。これはそういう使い方なので異常ではない。
        /// また枡のValueの値が0～9の範囲に無い場合も0を返す。同じく異常ではない。
        /// </remarks>
        private int ValueOf(Cell c)
        {
            return (c == null) ? 0 : ((c.Value < 0) ? 0 : c.Value);
        }

        /// <summary>
        /// 枡のValue2の値(0～9)を取得。Value2は繰り上がりや、繰り下がりとして使われる。
        /// </summary>
        /// <param name="c">枡のデータ</param>
        /// <returns>枡のValue2の値(0～9)</returns>
        /// <remarks>
        /// 枡がnullの場合は0を返す。これはそういう使い方なので異常ではない。
        /// また枡のValueの値が0～9の範囲に無い場合も0を返す。同じく異常ではない。
        /// </remarks>
        private int Value2Of(Cell c)
        {
            return (c == null) ? 0 : ((c.Value2 < 0) ? 0 : c.Value2);
        }

        /// <summary>
        /// 指定された場所にある数(複数の升目で構成される)の桁数を数える
        /// </summary>
        /// <param name="lineNo">数がある場所の行指定(0～)</param>
        /// <param name="fromRight">数がある場所の位置が右端から何番目か(0～)</param>
        /// <param name="width">数がある領域の幅(最大桁数)</param>
        /// <returns></returns>
        private int CheckLength(int lineNo, int fromRight, int width)
        {
            foreach (int i in Enumerable.Range(0, width))
            {
                int ix = numX - fromRight - i - 1;
                Cell c = cells.GetCellAt(ix, lineNo);
                if (c == null || c.Value < 0) return i; // 無効なCelを見つけたので、ここの右までで終わり
            }
            return width;
        }

        /// <summary>
        /// 升目の指定された位置に(複数桁の)数値を置く
        /// </summary>
        /// <param name="value">数値(複数桁でも良い)</param>
        /// <param name="lineNo">数値を置く場所の行(0～)</param>
        /// <param name="fromRight">数値の最小桁が升目の右端から何番目か(0～)</param>
        private void PutNumber(long value, int lineNo, int fromRight)
        {
            if (value < 0)      // 負の数の場合は、最初にマイナス記号を書いて、数値の符号を反転
            {
                cells.SetOperation(numX - fromRight - NumLength(value), lineNo, OpeType.Minus); ;
                value = -value;
            }
            foreach (int i in Enumerable.Range(0, NumLength(value)))
            {
                int ix = numX - fromRight - i - 1;
                int iy = lineNo;
                int d = DigitNum(value, i);
                cells.SetValue(ix, iy, d);
            }
        }

        /// <summary>
        /// 升目の指定された位置に演算記号を置く
        /// </summary>
        /// <param name="ope">演算の種類</param>
        /// <param name="lineNo">演算記号を置く場所の行指定(0～)</param>
        /// <param name="fromRight">演算記号を置く場所が升目の右端から何番目か(0～)</param>
        private void PutOperationMark(OpeType ope, int lineNo, int fromRight)
        {
            int ix = numX - fromRight - 1;
            int iy = lineNo;
            cells.SetOperation(ix, iy, ope);
        }

        /// <summary>
        /// 升目の指定された位置に下線を置く
        /// </summary>
        /// <param name="lineNo">下線を置く領域の行指定(0～)</param>
        /// <param name="fromRight">下線を置く領域の右端が升目の右端から何番目か(0～)</param>
        /// <param name="length">下線を置く領域の長さ</param>
        private void PutLine(int lineNo, int fromRight, int length)
        {
            foreach (int i in Enumerable.Range(0, length))
            {
                int ix = numX - fromRight - i - 1;
                int iy = lineNo;
                cells.SetUnderline(ix, iy, true);
            }
        }

        /// <summary>
        /// 与えられた数が何桁の数か求める
        /// </summary>
        /// <param name="v">数</param>
        /// <returns>桁数</returns>
        private int NumLength(long v)
        {
            return v.ToString().Length;
            // double用の関数 Math.Pow10 を整数にキャストするのは気分悪いので、ちゃっちゃと書いとこう。
            // この場合は、ちゃんと動くけど。
            // return (int)Math.Log10(v) + 1;
        }

        /// <summary>
        /// 数値の指定桁の値を得る
        /// </summary>
        /// <param name="v">対象となる数値</param>
        /// <param name="n">対象となる桁(0～)</param>
        /// <returns></returns>
        private int DigitNum(long v, int n)
        {
            if (NumLength(v) <= n) throw new Exception("その桁はない！"); // defensive.
            for (int i = 0; i < n; i++) v /= 10;
            return (int)(v % 10);
        }

        /// <summary>
        /// CellFieldに升目を描く
        /// </summary>
        private void DrawCellFrames()
        {
            foreach (int iy in Enumerable.Range(0, numY))
            {
                foreach (int ix in Enumerable.Range(0, numX))
                {
                    int x = cellPitchX * ix + leftMargin;
                    int y = cellPitchY * iy + upperMargin;
                    g.DrawRectangle(Pens.LightGray, x, y, cellWidth, cellHeight);
                }
            }
        }

        /// <summary>
        /// 表示の速度調整
        /// </summary>
        /// <param name="s">0～100の値。0側が速い</param>
        public void SetSpeed(int s)
        {
            waitControlValue = s;
        }

        /// <summary>
        /// セットされた升目のデータを描画し、与えられた時間だけ待つ(Sleepする)
        /// </summary>
        /// <param name="msec">待つ(Sleepする)時間をミリ秒で指定</param>
        private void UpdateWithDelay(int msec)
        {
            if (ABORT_FLAG)                             // UIスレッドからフラグがセットされていたら
            {
                ABORT_FLAG = false;                     // フラグを下ろして
                throw new Exception(ABORT_MESSAGE);     // 大域脱出として例外を上げる
            }
            DrawCells();                                // 升目をBitmapに描画し
            pictureBox.Invalidate();                    // UIスレッドに描画更新を依頼
            Thread.Sleep(msec * waitControlValue / W);  // 自分は所定の時間だけ寝る
        }

        /// <summary>
        /// 各升目のデータを描く
        /// </summary>
        private void DrawCells()
        {
            foreach (int iy in Enumerable.Range(0, numY))
            {
                foreach (int ix in Enumerable.Range(0, numX))
                {
                    Cell c = cells.GetCellAt(ix, iy);
                    if (c == null) continue;
                    DrawCell(ix, iy, c.Value, c.Value2, c.Ope, c.Underline);
                }
            }
        }

        /// <summary>
        /// CellFieldの指定された枡を表示する
        /// </summary>
        /// <param name="ix">枡の横位置(0～)</param>
        /// <param name="iy">枡の縦位置(0～)</param>
        /// <param name="v1">枡の中に書く値(0～9以外は書かない)</param>
        /// <param name="v2">枡の左上に書く値(0～9以外は書かない)</param>
        /// <param name="ope">演算子</param>
        /// <param name="underline">枡の下に線を引くかどうか</param>
        private void DrawCell(int ix, int iy, int v1, int v2, OpeType ope, bool underline)
        {
            int x = cellPitchX * ix + leftMargin;
            int y = cellPitchY * iy + upperMargin;

            if (0 <= v1)        // 枡の中の数字は0以上なら有効
            {
                g.DrawString(v1.ToString(), numFont, numBrush, x, y);
            }

            if (1 <= v2)        // 枡の左肩の数字は1以上なら書く
            {
                int upperLeftX = x - xMargin;
                int upperLeftY = y - (yMargin / 2);         // 線に掛からないよう少し低めに
                g.DrawString(v2.ToString(), numFont2, numBrush2, upperLeftX, upperLeftY);
            }

            if (ope == OpeType.Plus)  g.DrawString("+", numFont, numBrush, x, y);
            if (ope == OpeType.Minus) g.DrawString("-", numFont, numBrush, x, y);
            if (ope == OpeType.Mul)   g.DrawString("×", numFont, numBrush, x, y);
            if (ope == OpeType.Div)   DrawDivMark(x, y);

            if (underline)      // 下線を引く
            {
                int x1 = x - (xMargin / 2) - 1;             // 隣と線がつながるように -1
                int x2 = x + cellWidth + (xMargin / 2) + 1; // 同じく +1
                int y1 = y + cellHeight + (yMargin / 2);
                g.DrawLine(underLinePen, x1, y1, x2, y1);
            }
        }

        /// <summary>
        /// 割り算記号を書く
        /// </summary>
        /// <param name="x">記号を書く場所の横位置</param>
        /// <param name="y">記号を書く場所の縦位置</param>
        private void DrawDivMark(int x, int y)
        {
            int divw = cellWidth * 2;                       // 右下1/4だけ使うので、枡幅の2倍に
            int divh = cellHeight * 2 + yMargin;            // 右下1/4の高さが、枡高+マージンの半分 になるように
            int divx = x - cellWidth + (xMargin / 2);       // 右側にマージンの半分だけずらす
            int divy = y - cellHeight - yMargin;            // 下端が枡の下端にくるように
            g.DrawArc(underLinePen, divx, divy, divw, divh, 0, 90);
        }
    }
}
