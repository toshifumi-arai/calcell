//===================================================================
// 電卓もどきと筆算実行プログラム
// © 2016 Toshifumi Arai (toshifumi@acm.org)
//===================================================================

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalCell
{
    /// <summary>
    /// メインとなるフォーム。主に初期化とイベントのディスパッチ
    /// </summary>
    public partial class MainForm : Form
    {
        private Calc        calc;                   // 電卓もどき
        private CellField   cellField;              // 筆算を表示する升目

        private bool        caliculating = false;   // 筆算の途中ならtrue

        private static readonly String DoneMessage          = "筆算成功です！";
        private static readonly String NegValMessage        = "負の値は筆算では扱いません";
        private static readonly String AbortMessage         = "中止されました！";
        private static readonly String DivByZeroMessage     = "ゼロ除算です！";
        private static readonly String WritingErrorMessage  = "筆算途中でマス不足！";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 電卓もどきの初期設定
            calc = new Calc(NumTextBox, Button_PLUS, Button_MINUS, Button_MUL, Button_DIV);
            calc.DispText("0");

            // 筆算領域の初期設定
            cellField = new CellField(CellFieldPictureBox);
            cellField.Update(OpeType.Null, true, 0, 0, false);
            SpeedControl(); // cellFieldで筆算が実行される速さを設定しておく
        }

        /// <summary>
        /// 電卓側の状態に応じて、升目の領域を更新する
        /// </summary>
        /// <param name="go">計算までやるかどうか</param>
        public async void UpdateCellField(bool go)
        {
            long value1, value2;
            OpeType ope;
            bool waiting;
            calc.GetCurrentStat(out ope, out waiting, out value1, out value2);          // 電卓の状態を取得

            DisableButtons();                                                           // ボタン類をdisable

            await Task.Run(() => cellField.Update(ope, waiting, value1, value2, go));   // 升目の領域を更新
            // 筆算の表示には結構時間がかかるし、長い筆算の途中でスクロールしたくなる
            // こともあるので、筆算の表示は非同期で実行する。
            // ただし、計算に関わるボタン類はすべて押せないようにしている。
            // この間にできるのは、以下の操作だけ：
            // 　　・画面のスクロール
            // 　　・筆算表示速度の調整
            // 　　・筆算の中止(ABORTボタンで)
            // 　　・プログラムの終了(ウィンドウ枠の×印で)

            CellStat s = cellField.GetStat();
            if (s == CellStat.Ok)            CommentLabel.Text = "";
            if (s == CellStat.NegativeValue) CommentLabel.Text = NegValMessage;
            if (s == CellStat.Abort)         CommentLabel.Text = AbortMessage;
            if (s == CellStat.DivByZero)     CommentLabel.Text = DivByZeroMessage;
            if (s == CellStat.OutOfRange)    CommentLabel.Text =  WritingErrorMessage;

            if (s == CellStat.Ok && go)      CommentLabel.Text = DoneMessage;
            if (go) calc.DoCalculation();

            EnableButtons();                                                            // ボタン類をenable
        }

        /// <summary>
        /// 計算用のボタンやキー入力を使えないようにする
        /// </summary>
        private void DisableButtons()
        {
            caliculating = true;

            Button_0.Enabled = false;
            Button_1.Enabled = false;
            Button_2.Enabled = false;
            Button_3.Enabled = false;
            Button_4.Enabled = false;
            Button_5.Enabled = false;
            Button_6.Enabled = false;
            Button_7.Enabled = false;
            Button_8.Enabled = false;
            Button_9.Enabled = false;

            Button_BS.Enabled = false;
            Button_AC.Enabled = false;

            Button_PLUS.Enabled  = false;
            Button_MINUS.Enabled = false;
            Button_MUL.Enabled   = false;
            Button_DIV.Enabled   = false;

            Button_EQ.Enabled = false;
        }

        /// <summary>
        /// 計算用のボタンやキー入力を使えるようにする
        /// </summary>
        private void EnableButtons()
        {
            caliculating = false;

            Button_0.Enabled = true;
            Button_1.Enabled = true;
            Button_2.Enabled = true;
            Button_3.Enabled = true;
            Button_4.Enabled = true;
            Button_5.Enabled = true;
            Button_6.Enabled = true;
            Button_7.Enabled = true;
            Button_8.Enabled = true;
            Button_9.Enabled = true;

            Button_BS.Enabled = true;
            Button_AC.Enabled = true;

            Button_PLUS.Enabled  = true;
            Button_MINUS.Enabled = true;
            Button_MUL.Enabled   = true;
            Button_DIV.Enabled   = true;

            Button_EQ.Enabled = true;
        }

        /// <summary>
        /// 筆算を実行する速度を調整
        /// </summary>
        private void SpeedControl()
        {
            int min = SpeedBar.Minimum;
            int max = SpeedBar.Maximum - min;
            int val = SpeedBar.Value - min;
            cellField.SetSpeed(val * 100 / max);    // 0～100で速度調節(0側が速い)
        }

        /// <summary>
        /// SpeedBarやButton_ABORTからフォーカスを外す
        /// </summary>
        /// <remarks>
        /// SpeedBarからフォーカスを剥がさないと、スクロールホイールで画面のスクロールが出来ない。
        /// Button_ABORTにフォーカスがあると、ENTERで計算を始められない。
        /// フォーカスの行き先は何処でも良いようだが、とりあえずCellFieldPictureBoxにしておく。
        /// </remarks>
        private void ResetFocus()
        {
            CellFieldPictureBox.Focus();
        }

        // 数値入力ボタンのイベントハンドラ
        private void Button_0_Click(object sender, EventArgs e) { calc.NumInput(0); UpdateCellField(false); }
        private void Button_1_Click(object sender, EventArgs e) { calc.NumInput(1); UpdateCellField(false); }
        private void Button_2_Click(object sender, EventArgs e) { calc.NumInput(2); UpdateCellField(false); }
        private void Button_3_Click(object sender, EventArgs e) { calc.NumInput(3); UpdateCellField(false); }
        private void Button_4_Click(object sender, EventArgs e) { calc.NumInput(4); UpdateCellField(false); }
        private void Button_5_Click(object sender, EventArgs e) { calc.NumInput(5); UpdateCellField(false); }
        private void Button_6_Click(object sender, EventArgs e) { calc.NumInput(6); UpdateCellField(false); }
        private void Button_7_Click(object sender, EventArgs e) { calc.NumInput(7); UpdateCellField(false); }
        private void Button_8_Click(object sender, EventArgs e) { calc.NumInput(8); UpdateCellField(false); }
        private void Button_9_Click(object sender, EventArgs e) { calc.NumInput(9); UpdateCellField(false); }

        // 編集用ボタンのイベントハンドラ
        private void Button_BS_Click(object sender, EventArgs e) { calc.HandleBackSpace(); UpdateCellField(false); }
        private void Button_AC_Click(object sender, EventArgs e) { calc.HandleAllClear();  UpdateCellField(false); }

        // 演算子ボタンのイベントハンドラ
        private void Button_PLUS_Click(object sender, EventArgs e)  { calc.OpeInput(OpeType.Plus);  UpdateCellField(false); }
        private void Button_MINUS_Click(object sender, EventArgs e) { calc.OpeInput(OpeType.Minus); UpdateCellField(false); }
        private void Button_MUL_Click(object sender, EventArgs e)   { calc.OpeInput(OpeType.Mul);   UpdateCellField(false); }
        private void Button_DIV_Click(object sender, EventArgs e)   { calc.OpeInput(OpeType.Div);   UpdateCellField(false); }

        // '='が押されたときのイベントハンドラ
        private void Button_EQ_Click(object sender, EventArgs e) { UpdateCellField(true); }

        // SpeedBarで速度調節
        private void SpeedBar_Scroll(object sender, EventArgs e) { SpeedControl(); }
        private void SpeedBar_MouseLeave(object sender, EventArgs e) { ResetFocus(); }

        // 中止ボタンが押された時の処理
        private void Button_ABORT_Click(object sender, EventArgs e) { if (caliculating) cellField.Abort(); ResetFocus();  }

        // 何かキーが押されたとき
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (caliculating) return;

            if (e.KeyCode == Keys.Back)   Button_BS_Click(sender, e);
            if (e.KeyCode == Keys.Escape) Button_AC_Click(sender, e);
            if (e.KeyCode == Keys.Enter)  Button_EQ_Click(sender, e);
        }

        // 同じく
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (caliculating) return;

            if (e.KeyChar == '0') Button_0_Click(sender, e);
            if (e.KeyChar == '1') Button_1_Click(sender, e);
            if (e.KeyChar == '2') Button_2_Click(sender, e);
            if (e.KeyChar == '3') Button_3_Click(sender, e);
            if (e.KeyChar == '4') Button_4_Click(sender, e);
            if (e.KeyChar == '5') Button_5_Click(sender, e);
            if (e.KeyChar == '6') Button_6_Click(sender, e);
            if (e.KeyChar == '7') Button_7_Click(sender, e);
            if (e.KeyChar == '8') Button_8_Click(sender, e);
            if (e.KeyChar == '9') Button_9_Click(sender, e);

            if (e.KeyChar == '+') Button_PLUS_Click(sender, e);
            if (e.KeyChar == '-') Button_MINUS_Click(sender, e);
            if (e.KeyChar == '*') Button_MUL_Click(sender, e);
            if (e.KeyChar == '/') Button_DIV_Click(sender, e);

            if (e.KeyChar == '=') Button_EQ_Click(sender, e);
        }

    }
}
