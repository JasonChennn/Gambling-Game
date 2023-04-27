using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication6;

/*
 *  聲明：
 *  本小遊戲完完全全由 亞東技術學院 資2A 105111146 陳思杰親手設計製作及構思
 *  完全無任何抄襲或者COPY
 *  開始製作日期: 2017/12/2
 *  組員：陳思杰 (105111146)
 *  小組人數: 1
 */
namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        static int money = 0; //目前金錢
        static int[] odds = new int[6]; //賠率
        static int player_digital = 0; //玩家骰子點數
        static int originate = 50000; //起始金額
        static int[] effect = new int[3]; //效果
        static int rob = 0; //檢測玩家是否搶劫
        static int[] log = new int[5]; //紀錄
        static int cd = -1;
        Randomevent revent = new Randomevent();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //啟動讀取時
        {
            //清除label背景顏色
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label19.BackColor = Color.Transparent;
            label10.Text = "無"; label11.Text = "無"; label13.Text = "無"; //賠率效果顯示
            GiveMoney(originate); //給錢副程式
            label1.Text = "$"+money.ToString(); //更新金錢顯示
            OddsChange(); //改變賠率
            for(int i=0; i<effect.GetLength(0); i++) //所有效果初始值
            {
                effect[i] = 0;
            }
            for (int i = 0; i < log.GetLength(0); i++) //所有紀錄初始值
            {
                log[i] = 0;
            }
            LogUpdate(); //更新紀錄顯示(label)
        }

        public void OddsChange()
        {
            Random rate = new Random();
            odds[0] = rate.Next(1,5); // 猜拳賠率
            odds[1] = rate.Next(1,4); // 比大小賠率
            odds[2] = rate.Next(900000, 1000001); // 大樂透-頭獎獎金
            odds[3] = rate.Next(300000, 500001); // 大樂透-貳獎獎金
            odds[4] = rate.Next(50000, 100001); // 大樂透-叁獎獎金
            odds[5] = rate.Next(10000, 50001); // 大樂透-肆獎獎金

            //更新顯示(label)
            label2.Text = "賠率:"+odds[0].ToString();
            label3.Text = "賠率:" + odds[1].ToString();
            label6.Text = "$" + odds[2].ToString();
            label7.Text = "$" + odds[3].ToString();
            label8.Text = "$" + odds[4].ToString();
            label9.Text = "$" + odds[5].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cd > 0)
            {
                MessageBox.Show("您案的太快了,請等待 " + cd + " 秒.");
            }
            else
            {
                if (textBox1.Text.Trim() == String.Empty) //檢測textbox為空時
                {
                    MessageBox.Show("您沒有輸入賭金!");
                }
                else
                {
                    int stakes = int.Parse(textBox1.Text);
                    if (stakes <= 0 || money < stakes)
                    {
                        MessageBox.Show("賭金不能少於0或是您的賭金不足!");
                    }
                    else
                    {
                        Random mora = new Random();
                        int i = mora.Next(0, 3); //0為剪刀、1為石頭、2為布
                        if (i == 0)
                        {
                            MessageBox.Show("對方出的是剪刀，這局平手！！");
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else if (i == 1)
                        {
                            MessageBox.Show("對方出的是石頭，您輸了！並且損失賭金： " + stakes);
                            GiveMoney(-stakes);
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else
                        {
                            if (effect[0] == 1)
                            {
                                MessageBox.Show("對方出的是布，您贏了！並且獲得(效果)： " + 5 * stakes);
                                GiveMoney(5 * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                            else
                            {
                                MessageBox.Show("對方出的是布，您贏了！並且獲得： " + odds[0] * stakes);
                                GiveMoney(odds[0] * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                        }
                        effect[0] = 0;
                        label10.Text = "無";
                        log[0]++;
                        LogUpdate();
                        cd = 1;
                        revent.start();
                        RandomEvent();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cd > 0)
            {
                MessageBox.Show("您案的太快了,請等待 " + cd + " 秒.");
            }
            else
            {
                if (textBox1.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("您沒有輸入賭金!");
                }
                else
                {
                    int stakes = int.Parse(textBox1.Text);
                    if (stakes <= 0 || money < stakes)
                    {
                        MessageBox.Show("賭金不能少於0或是您的賭金不足!");
                    }
                    else
                    {
                        Random mora = new Random();
                        int i = mora.Next(0, 3); //0為石頭、1為布、2為剪刀
                        if (i == 0)
                        {
                            MessageBox.Show("對方出的是石頭，這局平手！！");
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else if (i == 1)
                        {
                            MessageBox.Show("對方出的是布，您輸了！並且損失賭金： " + stakes);
                            GiveMoney(-stakes);
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else
                        {
                            if (effect[0] == 1)
                            {
                                MessageBox.Show("對方出的是剪刀，您贏了！並且獲得(效果)： " + 5 * stakes);
                                GiveMoney(5 * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                            else
                            {
                                MessageBox.Show("對方出的是剪刀，您贏了！並且獲得： " + odds[0] * stakes);
                                GiveMoney(odds[0] * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                        }
                        effect[0] = 0;
                        label10.Text = "無";
                        log[0]++;
                        LogUpdate();
                        cd = 1;
                        revent.start();
                        RandomEvent();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cd > 0)
            {
                MessageBox.Show("您案的太快了,請等待 " + cd + " 秒.");
            }
            else
            {
                if (textBox1.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("您沒有輸入賭金!");
                }
                else
                {
                    int stakes = int.Parse(textBox1.Text);
                    if (stakes <= 0 || money < stakes)
                    {
                        MessageBox.Show("賭金不能少於0或是您的賭金不足!");
                    }
                    else
                    {
                        Random mora = new Random();
                        int i = mora.Next(0, 3); //0為布、1為剪刀、2為石頭
                        if (i == 0)
                        {
                            MessageBox.Show("對方出的是布，這局平手！！");
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else if (i == 1)
                        {
                            MessageBox.Show("對方出的是剪刀，您輸了！並且損失賭金： " + stakes);
                            GiveMoney(-stakes);
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                        }
                        else
                        {
                            if (effect[0] == 1)
                            {
                                MessageBox.Show("對方出的是石頭，您贏了！並且獲得(效果)： " + 5 * stakes);
                                GiveMoney(5 * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                            else
                            {
                                MessageBox.Show("對方出的是石頭，您贏了！並且獲得： " + odds[0] * stakes);
                                GiveMoney(odds[0] * stakes);
                                OddsChange();
                                label1.Text = "$" + money.ToString();
                            }
                        }
                        effect[0] = 0;
                        label10.Text = "無";
                        log[0]++;
                        LogUpdate();
                        cd = 1;
                        revent.start();
                        RandomEvent();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(cd > 0)
            {
                MessageBox.Show("您案的太快了,請等待 " + cd + " 秒.");
            }
            else
            {
                if (textBox2.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("您沒有輸入賭金!");
                }
                else
                {
                    int stakes = int.Parse(textBox2.Text);
                    if (stakes <= 0 || money < stakes)
                    {
                        MessageBox.Show("賭金不能少於0或是您的賭金不足!");
                    }
                    else
                    {
                        Random digital = new Random();
                        player_digital = digital.Next(1, 7);
                        MessageBox.Show("您擲出的骰子是: " + player_digital);
                        CompareDice(stakes); //輪到對手擲骰子時(副程式)
                        cd = 1;
                    }
                }
            }
        }
        public void CompareDice(int stakes)
        {
            Random digital = new Random();
            int bot_digital = digital.Next(1, 7);
            if(effect[1] == 1) { bot_digital = digital.Next(1, 5); }
            if (player_digital > bot_digital)
            {
                MessageBox.Show("對手擲出的骰子是:"+ bot_digital+",恭喜您獲得了賭金:"+ odds[1] * stakes);
                GiveMoney(odds[1] * stakes);
                OddsChange();
                label1.Text = "$"+money.ToString();
            }
            else if (player_digital < bot_digital)
            {
                MessageBox.Show("對手擲出的骰子是:" + bot_digital + ",您失去了:" + stakes);
                GiveMoney(-stakes);
                OddsChange();
                label1.Text = "$"+money.ToString();
            }
            else
            {
                MessageBox.Show("對手擲出的骰子是:" + bot_digital + ",平手！");
                OddsChange();
                label1.Text = "$"+money.ToString();
            }
            effect[1] = 0;
            label11.Text = "無";
            log[0]++;
            LogUpdate();
            revent.start();
            RandomEvent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cd > 0)
            {
                MessageBox.Show("您案的太快了,請等待 " + cd + " 秒.");
            }
            else
            {
                if (textBox3.Text.Trim() == String.Empty || textBox4.Text.Trim() == String.Empty || textBox5.Text.Trim() == String.Empty ||
                    textBox6.Text.Trim() == String.Empty || textBox7.Text.Trim() == String.Empty || textBox8.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("您的樂透號碼不完整!");
                }
                else
                {
                    if (money < 500 || money >= 500000)
                    {
                        MessageBox.Show("您沒有500元或者您身上的錢超過50萬!");
                    }
                    else
                    {
                        int[] number = new int[6];
                        Random digital = new Random();
                        if (effect[2] == 1)
                        {
                            for (int i = 0; i < number.GetLength(0); i++)
                            {
                                number[i] = digital.Next(1, 16);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < number.GetLength(0); i++)
                            {
                                number[i] = digital.Next(1, 31);
                            }
                        }
                        int sum = 0;
                        int number1 = int.Parse(textBox3.Text);
                        int number2 = int.Parse(textBox4.Text);
                        int number3 = int.Parse(textBox5.Text);
                        int number4 = int.Parse(textBox6.Text);
                        int number5 = int.Parse(textBox7.Text);
                        int number6 = int.Parse(textBox8.Text);
                        if (number1 < 1 || number1 > 30)
                        {
                            MessageBox.Show("您第一組的號碼範圍只能為(1-30)!");
                        }
                        else if (number2 < 1 || number2 > 30)
                        {
                            MessageBox.Show("您第二組的號碼範圍只能為(1-30)!");
                        }
                        else if (number3 < 1 || number3 > 30)
                        {
                            MessageBox.Show("您第三組的號碼範圍只能為(1-30)!");
                        }
                        else if (number4 < 1 || number4 > 30)
                        {
                            MessageBox.Show("您第四組的號碼範圍只能為(1-30)!");
                        }
                        else if (number5 < 1 || number5 > 30)
                        {
                            MessageBox.Show("您第五組的號碼範圍只能為(1-30)!");
                        }
                        else if (number6 < 1 || number6 > 30)
                        {
                            MessageBox.Show("您第六組的號碼範圍只能為(1-30)!");
                        }
                        else
                        {
                            if (number[0] == number1 || number[0] == number2 || number[0] == number3 || number[0] == number4 || number[0] == number5 || number[0] == number6) sum++;
                            if (number[1] == number1 || number[1] == number2 || number[1] == number3 || number[1] == number4 || number[1] == number5 || number[1] == number6) sum++;
                            if (number[2] == number1 || number[2] == number2 || number[2] == number3 || number[2] == number4 || number[2] == number5 || number[2] == number6) sum++;
                            if (number[3] == number1 || number[3] == number2 || number[3] == number3 || number[3] == number4 || number[3] == number5 || number[3] == number6) sum++;
                            if (number[4] == number1 || number[4] == number2 || number[4] == number3 || number[4] == number4 || number[4] == number5 || number[4] == number6) sum++;
                            if (number[5] == number1 || number[5] == number2 || number[5] == number3 || number[5] == number4 || number[5] == number5 || number[5] == number6) sum++;
                            if (sum == 0) MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您沒有中任何一支!銘謝惠顧");
                            else if (sum == 1) MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您只中了一支!銘謝惠顧");
                            else if (sum == 2) MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您只中了二支!銘謝惠顧");
                            else if (sum == 3) { MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您中了三支!並且獲得肆獎獎金:" + odds[5]); GiveMoney(odds[5]); log[3]++; LogUpdate(); }
                            else if (sum == 4) { MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您中了四支!並且獲得参獎獎金:" + odds[4]); GiveMoney(odds[4]); log[3]++; LogUpdate(); }
                            else if (sum == 5) { MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您中了五支!並且獲得貳獎獎金:" + odds[3]); GiveMoney(odds[3]); log[3]++; LogUpdate(); }
                            else { MessageBox.Show("大樂透號碼為:" + number[0] + "," + number[1] + "," + number[2] + "," + number[3] + "," + number[4] + "," + number[5] + "\n您中了六支!並且獲得頭獎獎金:" + odds[2]); GiveMoney(odds[2]); log[3]++; LogUpdate(); }
                            GiveMoney(-500);
                            OddsChange();
                            label1.Text = "$" + money.ToString();
                            effect[2] = 0;
                            label13.Text = "無";
                            log[1]++;
                            LogUpdate();
                            cd = 1;
                            revent.start();
                            RandomEvent();
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "admin" || textBox2.Text == "admin") { GiveMoney(100000); label1.Text = "$" + money.ToString(); }
            if (textBox2.Text == "event") { GiveMoney(100000); label1.Text = "$" + money.ToString(); }
            MessageBox.Show("您將從5萬元起家\n只要您能獲得100萬元以上您就過關獲勝\n您還能從商店購買一些效果幫助您達成獲勝!!","獲勝規則");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("每一局的賠率隨機(1-4倍)\n您若猜拳獲勝可以贏得賠率*賭金之金額\n反之輸了就失去賭金\n平手則不獲得也不損失賭金", "猜拳規則");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("每一局的賠率隨機(1-3倍)\n您先擲骰子\n接著對方擲骰子\n您的點數大於對方即獲勝可得賠率*賭金之金額\n反之輸了就失去賭金\n平手則不獲得也不損失賭金", "比大小規則");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("大樂透只要中三支以上即獲得獎金\n請注意!!\n超過五十萬元就無法遊玩大樂透\n請務必用您的賭術獲勝", "大樂透規則");
        }

        public void GiveMoney(int amount)
        {
            money += amount;
            if(money >= 1000000)
            {
                MessageBox.Show("恭喜您獲勝了!!\n您憑著賭術取得超過一百萬~");
                label19.Text = "過關";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("價格:5000元\n此效果只適合用在猜拳\n您的下一局猜拳裡賠率為5倍\n請注意,失敗或平手也仍然會消耗此效果\n請問您要購買此效果嗎?", "購買賠率增加",MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                if(rob == 1)
                {
                    MessageBox.Show("很抱歉\n因為您搶劫過店家\n店家不願意出售效果給您");
                }
                else
                {
                    if (money < 5000)
                    {
                        MessageBox.Show("您的金錢不足5000元~");
                    }
                    else
                    {
                        GiveMoney(-5000);
                        label1.Text = "$" + money.ToString();
                        odds[0] = 5;
                        label2.Text = "賠率:" + odds[0].ToString();
                        effect[0] = 1;
                        label10.Text = "啟用中";
                        log[2]++;
                        LogUpdate();
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("價格:20000元\n此效果只適合用在比大小\n您的對手在下一局骰子點數不會超過4\n請注意,失敗或平手也仍然會消耗此效果\n請問您要購買此效果嗎?", "購買灌鉛效果", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (rob == 1)
                {
                    MessageBox.Show("很抱歉\n因為您搶劫過店家\n店家不願意出售效果給您");
                }
                else
                {
                    if (money < 20000)
                    {
                        MessageBox.Show("您的金錢不足20000元~");
                    }
                    else
                    {
                        GiveMoney(-20000);
                        label1.Text = "$" + money.ToString();
                        effect[1] = 1;
                        label11.Text = "啟用中";
                        log[2]++;
                        LogUpdate();
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("價格:5000元\n此效果只適合用在大樂透\n大樂透的號碼於下一局內只會開出範圍(1-15)\n請注意,效果只有一次\n請問您要購買此效果嗎?", "購買透視效果", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (rob == 1)
                {
                    MessageBox.Show("很抱歉\n因為您搶劫過店家\n店家不願意出售效果給您");
                }
                else
                {
                    if (money < 5000)
                    {
                        MessageBox.Show("您的金錢不足5000元~");
                    }
                    else
                    {
                        GiveMoney(-5000);
                        label1.Text = "$" + money.ToString();
                        effect[2] = 1;
                        label13.Text = "啟用中";
                        log[2]++;
                        LogUpdate();
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("當您破產時,您可以選擇搶劫商店\n每次可以搶到25000~75000元不等\n請注意,當您搶劫後,店家將永遠不會再販售任何效果給您\n請問您要搶劫嗎?", "搶劫商店", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (money <= 0)
                {
                    rob = 1;
                    Random amount = new Random();
                    int robmoney = amount.Next(25000, 75001);
                    GiveMoney(robmoney);
                    label1.Text = "$" + money.ToString();
                    MessageBox.Show("您從店家裡搶了 " + robmoney + " 元\n店家將不會再出售效果給您!");
                    log[4]++;
                    LogUpdate();
                }
                else
                {
                    MessageBox.Show("您還沒破產\n所以搶劫無效");
                }
            }
        }

        public void LogUpdate()
        {
            label14.Text = log[0].ToString();
            label15.Text = log[1].ToString();
            label16.Text = log[2].ToString();
            label17.Text = log[3].ToString();
            label18.Text = log[4].ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您確定要存檔嗎?\n(若您存檔之後,上一個保存紀錄即會被覆蓋)", "保存紀錄", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string path = "load.ini";
                StreamWriter load = new StreamWriter(path,false);
                load.WriteLine(money);// 金錢
                load.WriteLine(effect[0]);// 賠率效果
                load.WriteLine(effect[1]);// 灌鉛效果
                load.WriteLine(effect[2]);// 透視效果
                load.WriteLine(log[0]);// 賭博次數
                load.WriteLine(log[1]);// 購買樂透次數
                load.WriteLine(log[2]);// 購買次數
                load.WriteLine(log[3]);// 中樂透次數
                load.WriteLine(log[4]);// 搶劫次數
                load.Close();
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您確定要讀取紀錄嗎?", "讀取紀錄", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string path = "load.ini";
                if (!System.IO.File.Exists(path))
                {
                    MessageBox.Show(path + " 您沒有保存紀錄,無法讀檔");
                    return;
                }
                string line;
                int[] data = new int[9];
                int count = 0;
                StreamReader load = new StreamReader(path);
                while ((line = load.ReadLine()) != null)
                {
                    data[count] = int.Parse(line);
                    Console.WriteLine(line);
                    count++;
                }
                money = 0;
                GiveMoney(data[0]);
                effect[0] = data[1];
                effect[1] = data[2];
                effect[2] = data[3];
                log[0] = data[4];
                log[1] = data[5];
                log[2] = data[6];
                log[3] = data[7];
                log[4] = data[8];
                label1.Text = "$" + money.ToString();
                if (effect[0] == 1) { label10.Text = "啟用中"; } else { label10.Text = "無"; }
                if (effect[1] == 1) { label11.Text = "啟用中"; } else { label11.Text = "無"; }
                if (effect[2] == 1) { label13.Text = "啟用中"; } else { label13.Text = "無"; }
                if(log[4] != 0) { rob = 1; }
                LogUpdate();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(cd > 0)
            {
                cd--;
            }
            else
            {
                cd = -1;
            }
        }

        public void RandomEvent ()
        {
            Random r = new Random();
            int events = r.Next(0, 6);
            int times = revent.gettimes();
            if (times >= 5)
            {
                if(events == 0)
                {
                    GiveMoney(5000);
                    label1.Text = "$" + money.ToString();
                    MessageBox.Show("[隨機事件]:財神爺到!您得到了 " + "5000" + " 元");
                }
                if (events == 1)
                {
                    GiveMoney(-2500);
                    label1.Text = "$" + money.ToString();
                    MessageBox.Show("[隨機事件]:真倒楣!您被敲竹槓，失去了 " + "2500" + " 元");
                }
                if (events == 2)
                {
                    GiveMoney(10);
                    label1.Text = "$" + money.ToString();
                    MessageBox.Show("[隨機事件]:有人覺得您很可憐!給了您 " + "10" + " 元");
                }
                if (events == 3)
                {
                    int lost = r.Next(1, 10000);
                    GiveMoney(-lost);
                    label1.Text = "$" + money.ToString();
                    MessageBox.Show("[隨機事件]:有人搶了您的錢!您失去了 " + lost + " 元");
                }
                revent.resettimes();
            }
        }
    }
}
