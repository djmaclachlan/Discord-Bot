using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordBotApplication
{
    public partial class AddPlayer : Form
    {
        public AddPlayer()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
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
                        cmd.CommandText = "insert into Players ([playerName], [archetype], [descriptor], [focus], [intMax], [mightMax], [speedMax], [intCurrent], [mightCurrent], [speedCurrent]) values " +
                                    "('" + txtPlayerName.Text + "', '" + txtArchetype.Text + "', '" + txtDescriptor.Text + "', '" +
                                    txtFocus.Text + "', " + txtIntelligence.Text + ", " + txtMight.Text + ", " + txtSpeed.Text + ", " +
                                    txtIntelligence.Text + ", " + txtMight.Text + ", " + txtSpeed.Text + ");";
                        Console.WriteLine(Convert.ToString(cmd.CommandText));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("New Player Has Been Added to the Database!");
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
