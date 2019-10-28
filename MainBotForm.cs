using System.Windows.Forms;
using System.Windows;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SQLite;
using System.Data;
using System.Data.OleDb;

namespace DiscordBotApplication
{
    public partial class MainBotForm : Form
    {
        DiscordSocketClient Client;
        CommandHandler Handler;
        public MainBotForm()
        {
            InitializeComponent();
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
        }
        

        private async void button1_Click(object sender, EventArgs e)
        {
            Handler = new CommandHandler();
            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance
            }) ;

            await Handler.Install(Client);
            Client.Log += Client_Log;
            try
            {
                await Client.LoginAsync(TokenType.Bot, tbToken.Text);
                await Client.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            await Task.Delay(-1);
            
        }
        private Task Client_Log(LogMessage arg)
        {
            Invoke((Action)delegate
            {
                tbOutput.AppendText(arg + "\n");
            });
            return null;
        }
        private async void PublicRoll(int sides)
        {
            Random random = new Random();
            int output = random.Next(1, sides);
            Invoke((Action)delegate
            {
                tbOutput.AppendText("The Bot Has Rolled A [" + output + "]\n");
            });
            var channel = Client.GetChannel("Insert-Channel-ID-Here") as SocketTextChannel;
            await channel.SendMessageAsync("The GM has rolled a " + sides + " sided die and the result is [" + output + "]");
        }
        private void PrivateRoll(int sides)
        {
            Random random = new Random();
            int output = random.Next(1, sides);
            Invoke((Action)delegate
            {
                tbOutput.AppendText("The Bot Has Rolled A [" + output + "]\n");
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
              PrivateRoll(Convert.ToInt32(tbSideCount.Text));
            }
            catch (Exception ex)
            {
                tbOutput.AppendText("An Error Occured in your roll attempt\n" + ex + "\nPlease Enter A Valid Integer");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                PublicRoll(Convert.ToInt32(tbSideCount.Text));
            }
            catch (Exception ex)
            {
                tbOutput.AppendText("An Error Occured in your roll attempt\n" + ex + "\nPlease Enter A Valid Integer");
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(System.Drawing.Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void MainBotForm_Load(object sender, EventArgs e)
        {
            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.AutoGenerateColumns = true;
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                String strSQL = "select * from Players;";
                using (OleDbDataAdapter da = new OleDbDataAdapter(new OleDbCommand(strSQL, conn)))
                {
                    using (DataTable dtRecord = new DataTable())
                    {
                        da.Fill(dtRecord);
                        dataGridView1.DataSource = dtRecord;
                    }
                }
                conn.Close();

            }
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                String strSQL = "select * from NPCs;";
                using (OleDbDataAdapter da = new OleDbDataAdapter(new OleDbCommand(strSQL, conn)))
                {
                    using (DataTable dtRecord = new DataTable())
                    {
                        da.Fill(dtRecord);
                        dataGridView2.DataSource = dtRecord;
                    }
                }
                conn.Close();
            }
        }

        private void btnAddPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer frm1 = new AddPlayer();
            frm1.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {//Principal Due
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Update Players set intMax = intMax+1";
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    String strSQL =
                        "select * from players where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString(); ;

                    using (OleDbDataAdapter borrower_id = new OleDbDataAdapter(new OleDbCommand(strSQL, conn)))
                    {
                        DataSet borrowerDS = new DataSet();
                        borrower_id.Fill(borrowerDS);
                        foreach (DataTable table in borrowerDS.Tables)
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                lblName.Text = Convert.ToString(dr["playerName"]);
                                lblArchetype.Text = Convert.ToString(dr["archetype"]);
                                lblDescriptor.Text = Convert.ToString(dr["descriptor"]);
                                lblFocus.Text = Convert.ToString(dr["focus"]);
                                lblEffort.Text = Convert.ToString(dr["effort"]);
                                lblIntEdge.Text = Convert.ToString(dr["intEdge"]);
                                lblIntPoolCurrent.Text = Convert.ToString(dr["intCurrent"]);
                                lblIntPoolMax.Text = Convert.ToString(dr["intMax"]);
                                lblMightEdge.Text = Convert.ToString(dr["mightEdge"]);
                                lblMightPoolCurrent.Text = Convert.ToString(dr["mightCurrent"]);
                                lblMightPoolMax.Text = Convert.ToString(dr["mightMax"]);
                                lblSpeedEdge.Text = Convert.ToString(dr["speedEdge"]);
                                lblSpeedPoolCurrent.Text = Convert.ToString(dr["speedCurrent"]);
                                lblSpeedPoolMax.Text = Convert.ToString(dr["speedMax"]);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
