//===================================================================
// 電卓もどきと筆算実行プログラム
// © 2016 Toshifumi Arai (toshifumi@acm.org)
//===================================================================

using System;
using System.Windows.Forms;
using System.Drawing;

namespace CalCell
{
    /// <summary>
    /// 演算子の種類。四則演算と、未設定をあらわすNull。
    /// </summary>
    enum OpeType { Null, Plus, Minus, Mul, Div };

    /// <summary>
    /// 電卓のまねをするクラス。ただし扱う値は整数(long = 64ビット符号付整数)のみ！
    /// </summary>
    class Calc
    {
        // 電卓もどきの内部状態
        private bool    WaitingForTheNextValue = true;      // 次の数値待ちのときtrue
        private long    CurrentValue = 0;                   // 現在入力中の数値
        private long    PreviousValue = 0;                  // 先に入力済みの数値
        private OpeType CurrentOperation = OpeType.Null;    // 現在セットされている演算

        // 桁が溢れそうな場合の判定用定数と色
        private static readonly int     MaxDigitNum = 19;   // 64ビット符号付整数の最大桁数
        private static readonly Color   NormalBackColor = Color.WhiteSmoke;
        private static readonly Color   MaxDigitBackColor = Color.LightPink;

        // 表示用UI部品とメッセージや色
        private TextBox NumTextBox;                         // 数値やメッセージを表示するTextBox
        private Button  Button_PLUS, Button_MINUS, Button_MUL, Button_DIV; // ボタン類
        private static readonly String  OverflowMessage         = "桁あふれです！";
        private static readonly String  DivByZeroMessage        = "ゼロ除算です！";
        private static readonly String  UnknownOperationMessage = "未定義の演算です！";
        private static readonly Color   NormalOpeColor          = Color.Linen;
        private static readonly Color   SelectedOpeColor        = Color.Azure;

        /// <summary>
        /// UIのパーツを登録しつつインスタンスを初期化
        /// </summary>
        /// <param name="textBox">数値やメッセージを表示するテキストボックス</param>
        /// <param name="plusButton">'+'ボタン</param>
        /// <param name="minusButton">'-'ボタン</param>
        /// <param name="mulButton">'*'ボタン</param>
        /// <param name="divButton">'/'ボタン</param>
        /// <remarks>
        /// 演算子ボタンは、選択中の演算子の色を変えるために登録してもらう。
        /// </remarks>
        public Calc(TextBox textBox, Button plusButton, Button minusButton, Button mulButton, Button divButton)
        {
            NumTextBox   = textBox;
            Button_PLUS  = plusButton;
            Button_MINUS = minusButton;
            Button_MUL   = mulButton;
            Button_DIV   = divButton;
            ResetOperation();    // ボタンの色をすべて非選択色にしておく
        }
        
        /// <summary>
        /// 現在の状態を読み出す
        /// </summary>
        /// <param name="ope">現在セットされている演算子</param>
        /// <param name="waiting">新しい数値の入力待ちか</param>
        /// <param name="value1">現在の数値</param>
        /// <param name="value2">現在の数値(先に入力されてる方)</param>
        /// <remarks>
        /// 通常は入力を一文字処理した後にこれを呼ぶ。
        /// ただし '=' だけは、処理する前にこれを呼ぶ(演算子とかがクリアされるため)。
        /// </remarks>
        public void GetCurrentStat(out OpeType ope, out bool waiting, out long value1, out long value2)
        {
            ope     = CurrentOperation;
            waiting = WaitingForTheNextValue;
            value1  = CurrentValue;
            value2  = PreviousValue;
        }

        /// <summary>
        /// 数字ボタンが押されたときの処理
        /// </summary>
        /// <param name="v">押された数字(0～9)</param>
        public void NumInput(long v)
        {
            if (v < 0 || 9 < v) throw new Exception("範囲外！");    // defensive.

            if (WaitingForTheNextValue)         // 次の数値待ちの状態だったら...
            {
                PreviousValue = CurrentValue;   // 現在の値を保存して
                CurrentValue = 0;               // 現在の値は0から始める
                WaitingForTheNextValue = false; // 入力途中になったのでfalseに
            }
            try
            {
                checked                         // 数値計算のエラーをチェックさせるおまじない
                {
                    CurrentValue *= 10;         // 今のところは十進数だけ扱うと決めうち
                    CurrentValue += v;
                }
            }
            catch (OverflowException ex)        // 桁あふれが起こったら...
            {
                CurrentValue = 0;
                DispText(OverflowMessage);
                return;
            }
            DispText(CurrentValue.ToString());
        }

        /// <summary>
        /// 演算子が押されたときの処理
        /// </summary>
        /// <param name="ope">演算子の種類</param>
        public void OpeInput(OpeType ope)
        {
            if (CurrentOperation != OpeType.Null && !WaitingForTheNextValue) DoCalculation();
            // 1+2*3のように続けて演算する場合は、2つめの演算子('*')が来たとき、先の分(1+2)は計算してしまう。
            // このため、1+2*3と入力されたときの答えは(1+2)*3で9となる。普通(カシオとか)の電卓も同様の結果。
            // この方式では、1+2*まで入力した時点で、画面には3が表示される。
            // 一方、iPhoneの電卓アプリでは、1+2*3と入力すると、1+(2*3)とみなされ、答えは7になる。
            // その方式では、'*'を入力した時点では中間結果も表示されず...
            // もちろんそういう考え方もあるが、電卓としての使い勝手を考えれば、普通(カシオとか)の方が良いと考える。

            ResetOperation();
            // 現在セットされている演算子があっても、いったんキャンセルする。
            // 続けて演算子が入力されたら、最後のだけを有効にするため。
            // 2+-*3は、2*3とみなして6になる。普通(カシオとか)の電卓も、iPhoneの電卓アプリも、同様の処理。
            // 実はこれをやらなくても結果は同じなのだが、演算子のボタンの選択色を消すために入れている。

            CurrentOperation = ope;
            WaitingForTheNextValue = true;
            switch (ope)    // セットされた演算子のボタンを選択色にする。
            {
                case OpeType.Plus:
                    Button_PLUS.BackColor = SelectedOpeColor;
                    break;
                case OpeType.Minus:
                    Button_MINUS.BackColor = SelectedOpeColor;
                    break;
                case OpeType.Mul:
                    Button_MUL.BackColor = SelectedOpeColor;
                    break;
                case OpeType.Div:
                    Button_DIV.BackColor = SelectedOpeColor;
                    break;
                default:    // defensive. ここには来ないはず
                    throw new Exception("知らない演算子！");
                    break;
            }
        }

        /// <summary>
        /// BSボタンが押されたときの処理。数値入力中なら一文字消し。そうでないときはクリアと同じ
        /// </summary>
        public void HandleBackSpace()
        {
            if (WaitingForTheNextValue)
            {
                CurrentValue = 0;   // 次の数値を待っている時は、現在表示されている値は使わない。
                ResetOperation();   // 選択中の演算子があれば、それもクリア
            }

            CurrentValue /= 10;     // 整数値しか扱わないので、一の位を消すだけで一文字消しになる。
            DispText(CurrentValue.ToString());
        }

        /// <summary>
        /// ACボタンが押されたときの処理。すべてクリア
        /// </summary>
        public void HandleAllClear()
        {
            WaitingForTheNextValue = true;
            CurrentValue = 0;
            DispText(CurrentValue.ToString());
            ResetOperation();
        }

        /// <summary>
        /// 計算を実行する(数値計算のエラーはチェックされる)
        /// </summary>
        public void DoCalculation()
        {
            try
            {
                checked                     // 数値計算のエラーをチェックさせるおまじない
                {                           // 桁あふれも、ゼロ除算も、例外が上がるのを捕まえる
                    switch (CurrentOperation)
                    {
                        case OpeType.Null:  // 演算子を入力せず、いきなり '=' を押したとき
                            break;
                        case OpeType.Plus:
                            CurrentValue = PreviousValue + CurrentValue;
                            break;
                        case OpeType.Minus:
                            CurrentValue = PreviousValue - CurrentValue;
                            break;
                        case OpeType.Mul:
                            CurrentValue = PreviousValue * CurrentValue;
                            break;
                        case OpeType.Div:
                            CurrentValue = PreviousValue / CurrentValue;
                            break;
                        default:            // defensive
                            throw new Exception("知らない演算子！");
                            break;
                    }
                }
                DispText(CurrentValue.ToString());
            }
            catch (OverflowException ex)    // 桁あふれの処理はここで
            {
                DispText(OverflowMessage);
                CurrentValue = 0;
            }
            catch (System.DivideByZeroException ex)
            {
                DispText(DivByZeroMessage);
                CurrentValue = 0;
            }
            catch                           // defensive. ありえない演算子が来たときはここ。
            {
                DispText(UnknownOperationMessage);
                CurrentValue = 0;
            }

            WaitingForTheNextValue = true;
            ResetOperation();
        }

        /// <summary>
        /// 文字列をテキストボックスに表示しカーソルを末尾に
        /// </summary>
        /// <param name="s"></param>
        public void DispText(String s)
        {
            if (MaxDigitNum <= s.Length)
                NumTextBox.BackColor = MaxDigitBackColor;
            else
                NumTextBox.BackColor = NormalBackColor;
            NumTextBox.Text = s;
            NumTextBox.Select(NumTextBox.Text.Length, 0);   // カーソルを末尾に
        }

        /// <summary>
        /// 現在セットされている演算子をキャンセルし、ボタンの選択色も消す。
        /// </summary>
        private void ResetOperation()
        {
            CurrentOperation = OpeType.Null;
            Button_PLUS.BackColor   = NormalOpeColor;
            Button_MINUS.BackColor  = NormalOpeColor;
            Button_MUL.BackColor    = NormalOpeColor;
            Button_DIV.BackColor    = NormalOpeColor;
        }
    }
}
