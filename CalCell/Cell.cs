//===================================================================
// 電卓もどきと筆算実行プログラム
// © 2016 Toshifumi Arai (toshifumi@acm.org)
//===================================================================

namespace CalCell
{
    /// <summary>
    /// 二次元の升目のデータを保持するクラス
    /// </summary>
    class CellMap
    {
        Cell[,] cells;      // データは二次元配列で保持
        int numX, numY;     // 二次元配列の幅と高さ

        /// <summary>
        /// コンストラクタ。データの二次元配列を用意する
        /// </summary>
        /// <param name="nx">升目の横方向の個数</param>
        /// <param name="ny">升目の縦方向の個数</param>
        public CellMap(int nx, int ny)
        {
            cells = new Cell[nx, ny];
            numX = nx;
            numY = ny;
        }

        /// <summary>
        /// サイズは保持したまま、中身をクリア
        /// </summary>
        public void Clear()
        {
            cells = new Cell[numX, numY];   // 古いのは捨てて、新たしい二次元配列を確保
        }

        /// <summary>
        /// 指定した場所に値をセット
        /// </summary>
        /// <param name="ix">X方向の場所(0 ～ numX-1)</param>
        /// <param name="iy">Y方向の場所(0 ～ numY-1)</param>
        /// <param name="value">セットする値</param>
        public void SetValue(int ix, int iy, int value)
        {
            // 範囲チェックをしないので、はみ出すとIndexOutOfRangeの例外が出る。
            // 筆算の途中で枡が足りなくなると、はみ出すことになる。
            // その例外は、CellField.csのUpdateで捕らえられる。
            // 他のSetXxxx()でも同様。

            Cell c = cells[ix, iy];
            if (c == null) c = cells[ix, iy] = new Cell();  // nullだったら新しく確保
            c.Value = value;
        }

        /// <summary>
        /// 指定した場所に値2(繰り上がり、繰り下がり)をセット
        /// </summary>
        /// <param name="ix">X方向の場所(0 ～ numX-1)</param>
        /// <param name="iy">Y方向の場所(0 ～ numY-1)</param>
        /// <param name="value2">セットする値2(繰り上がり、繰り下がり)</param>
        public void SetValue2(int ix, int iy, int value2)
        {
            Cell c = cells[ix, iy];
            if (c == null) c = cells[ix, iy] = new Cell();
            c.Value2 = value2;
        }

        /// <summary>
        /// 指定した場所に演算子をセット
        /// </summary>
        /// <param name="ix">X方向の場所(0 ～ numX-1)</param>
        /// <param name="iy">Y方向の場所(0 ～ numY-1)</param>
        /// <param name="ope">演算子の種類</param>
        public void SetOperation(int ix, int iy, OpeType ope)
        {
            Cell c = cells[ix, iy];
            if (c == null) c = cells[ix, iy] = new Cell();
            c.Ope = ope;
        }

        /// <summary>
        /// 指定した場所に下線を引くよう指示
        /// </summary>
        /// <param name="ix">X方向の場所(0 ～ numX-1)</param>
        /// <param name="iy">Y方向の場所(0 ～ numY-1)</param>
        /// <param name="underline">下線を引くかどうか</param>
        public void SetUnderline(int ix, int iy, bool underline)
        {
            Cell c = cells[ix, iy];
            if (c == null) c = cells[ix, iy] = new Cell();
            c.Underline = underline;
        }

        /// <summary>
        /// 指定された位置の升目データを返す
        /// </summary>
        /// <param name="ix">X方向の場所</param>
        /// <param name="iy">Y方向の場所</param>
        /// <returns>指定された位置の升目データ</returns>
        public Cell GetCellAt(int ix, int iy)
        {
            // 範囲外のCellを要求されたらnullを返す。繰り上がりの参照時に、そういうこともあるので。
            if (ix < 0 || numX <= ix || iy < 0 || numY <= iy) return null;
            return cells[ix, iy];
        }
    }

    /// <summary>
    /// 各升目のデータを保持するクラス
    /// </summary>
    class Cell
    {
        // とりあえず、メンバを全部publicにして、勝手にアクセスしてもらう...

        public OpeType  Ope;        // 升目に書くのが演算子の場合、その種類
        public int      Value;      // 升目の値。0～9なら有効、それ以外は無視
        public int      Value2;     // 升目の左肩に小さく書く数字。0以上なら有効、それ以外は無視
        public bool     Underline;  // 升目の下に下線を引くかどうか

        public Cell()
        {
            Ope = OpeType.Null;     // 未設定。無視されるように
            Value = -1;             // 無効な数字。無視されるように
            Value2 = -1;            // 同じく
            Underline = false;      // 下線なし
        }
    }
}
