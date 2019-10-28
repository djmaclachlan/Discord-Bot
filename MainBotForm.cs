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
using System.Collections.Generic;
using System.Linq;

namespace DiscordBotApplication
{
    public partial class MainBotForm : Form
    {
        public string guild, channel;
        DiscordSocketClient Client;
        CommandHandler Handler;
        public MainBotForm()
        {
            InitializeComponent();
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
        }

        private async void SendMessage(string a) // Used for all custom messages sent
        {
            var channel1 = Client.GetChannel(Convert.ToUInt64(txtChannelID.Text)) as SocketTextChannel;
            await channel1.SendMessageAsync(a);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            guild = tbToken.Text;
            Handler = new CommandHandler();
            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance
            });

            await Handler.Install(Client);
            Client.Log += Client_Log;
            try
            {
                await Client.LoginAsync(TokenType.Bot, tbToken.Text);
                await Client.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error connecting to that token, please verify and try again.");
                Console.WriteLine(ex);
                return;
            }


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
            var channel1 = Client.GetChannel(Convert.ToUInt64(txtChannelID.Text)) as SocketTextChannel;
            await channel1.SendMessageAsync("The GM has rolled a " + sides + " sided die and the result is [" + output + "]");
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
        private void GetPlayers()
        {
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
        }
        private void GetNPCS()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";

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
        private void MainBotForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'universalDataSet.channels' table. You can move, or remove it, as needed.
            dataGridView2.AutoGenerateColumns = true;
            dataGridView1.AutoGenerateColumns = true;
            GetNPCS();
            GetPlayers();
        }

        private void btnAddPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer frm1 = new AddPlayer();
            frm1.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            GetNPCS();
            GetPlayers();
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

        private void a(object sender, EventArgs e)
        {

        }

        private void btnIncInt_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set intMax = intMax+1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum intelligence increased by 1!");
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

        private void btnDecInt_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set intMax = intMax-1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum intelligence decreased by 1!");
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

        private void btnIncMightMax_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set mightMax = mightMax+1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum might increased by 1!");
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


        private void btnDecMightMax_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set mightMax = mightMax-1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum might decreased by 1!");
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

        private void btnIncAgility_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set agilityMax = agilityMax+1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum agility increased by 1!");
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

        private void btnDecAgilityMax_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update players set agilityMax = agilityMax-1 where ID =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        SendMessage("Player Character " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has had his/her maximum agility decreased by 1!");
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            string cypherName = "", cypherDesc = "";
            Random random = new Random();
            int numberOfCyphers = 0;
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\dmaclachlan\source\repos\DiscordBotApplication\discordNetBotDB.accdb";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                String strSQL = "select count(*) as [Number of Cyphers] from Cyphers";
                using (OleDbDataAdapter da = new OleDbDataAdapter(new OleDbCommand(strSQL, conn)))
                {
                    using (DataTable dtRecord = new DataTable())
                    {
                        da.Fill(dtRecord);
                        foreach (DataRow dr in dtRecord.Rows)
                        {
                            numberOfCyphers = Convert.ToInt32(dr["Number of Cyphers"]);
                        }
                    }
                }
                conn.Close();
            }
            int output = random.Next(1, numberOfCyphers);
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                String strSQL = "select cypherName, cypherDescription from Cyphers where cypherID = " + output;
                using (OleDbDataAdapter da = new OleDbDataAdapter(new OleDbCommand(strSQL, conn)))
                {
                    using (DataTable dtRecord = new DataTable())
                    {
                        da.Fill(dtRecord);
                        foreach (DataRow dr in dtRecord.Rows)
                        {
                            cypherName = Convert.ToString(dr["cypherName"]);
                            cypherDesc = Convert.ToString(dr["cypherDescription"]);
                        }
                    }
                }
                conn.Close();
            }
            SendMessage(dataGridView1.CurrentRow.Cells[1].Value.ToString() + " has been awarded the folllowing cypher.\nCypher Name: " + cypherName + "\nCypher Description: " + cypherDesc);
        }

        private void btnChannelMessage_Click(object sender, EventArgs e)
        {
            SendMessage(txtMessage.Text);
        }
    }
}
