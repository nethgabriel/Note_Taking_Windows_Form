using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Note_Taking_Windows_Form.Global_Connection;

namespace Note_Taking_Windows_Form
{
    public partial class notesWin : Form
    {
        public notesWin()
        {
            InitializeComponent();
        }

        string userstr = "";
        private void dataGridViewMessages_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewMessages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dataGridViewMessages.SelectedRows.Count > 0)
            {
                userstr = dataGridViewMessages.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void loaddata()
        {
            con.Open(constr);
            cmd.ActiveConnection = con;

            //clears current loaded data on data grid to refresh
            dataGridViewMessages.Rows.Clear();

            //retrieves title of notes from database
            cmd.CommandText = "SELECT noteTitle FROM DataTable";
            rs = cmd.Execute(out object recordsaffected, Type.Missing, -1);

            //loads the following recorded titles until end of file
            while (!rs.EOF)
            {
                dataGridViewMessages.Rows.Add(rs.Fields[0].Value);
                rs.MoveNext();
            }

            con.Close();

        }

        private void entryReset()
        {
            textBoxTitle.Text = "";
            textBoxMessage.Text = "";
            dataGridViewMessages.SelectionMode = 0;
        }

        //created to limit line of code
        private void validation()
        {
            //confirmation for creating new notes
            DialogResult validation = MessageBox.Show("Clear existinig notes?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (validation == DialogResult.No)
            {
                con.Close();
            }
            else
            {
                entryReset();
                MessageBox.Show("Notes cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
            }
        }

        private void notesWin_Load(object sender, EventArgs e)
        {
            loaddata();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                //checks whether text box's have information
                if (textBoxTitle.Text == "" && textBoxMessage.Text == "")
                {

                }
                else
                {
                    con.Open(constr);
                    cmd.ActiveConnection = con;

                    cmd.CommandText = $"SELECT * FROM DataTable WHERE noteTitle = '{textBoxTitle.Text}'";
                    rs = cmd.Execute(out object recordsaffected, Type.Missing, -1);

                    if (!rs.EOF)
                    {
                        cmd.CommandText = $"SELECT * FROM DataTable WHERE noteMsg = '{textBoxMessage.Text}'";
                        rs = cmd.Execute(out object Recordsaffected, Type.Missing, -1);

                        if (!rs.EOF)
                        {
                            entryReset();
                            con.Close();
                        }
                        else
                        {
                            validation();
                        }

                        
                    }
                    else
                    {
                        validation();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                //assess if  a note have a title
                if (textBoxTitle.Text == "")
                {
                    MessageBox.Show("Title field required.", "Missing Field", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    //saves note to db
                    con.Open(constr);
                    cmd.ActiveConnection = con;

                    cmd.CommandText = $"INSERT INTO DataTable (noteTitle, noteMsg) VALUES ('{textBoxTitle.Text}', '{textBoxMessage.Text}')";
                    rs = cmd.Execute(out object RecordsAffected, Type.Missing, -1);

                    MessageBox.Show("Notes Saved", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    con.Close();

                    entryReset();

                    loaddata();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open(constr);
                cmd.ActiveConnection = con;

                //reads cell content and pastes to title
                textBoxTitle.Text = dataGridViewMessages.CurrentCell.Value.ToString();
                //selects message connected to the selected title
                cmd.CommandText = $"SELECT noteMsg FROM DataTable WHERE noteTitle = '{textBoxTitle.Text}'";
                rs = cmd.Execute(out object recordsaffected, Type.Missing, -1);

                textBoxMessage.Text = rs.Fields["noteMsg"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            con.Open(constr);
            cmd.ActiveConnection = con;

            try
            {
                if (dataGridViewMessages.SelectedRows.Count > 0)
                {
                    DialogResult confirmation = MessageBox.Show("Are you sure you want to remove note?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmation == DialogResult.No)
                    {
                        MessageBox.Show("Deletion Cancelled.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        entryReset();
                        con.Close();
                    }
                    else
                    {
                        cmd.CommandText = $"DELETE * FROM DataTable WHERE noteTitle = '{userstr}'";
                        cmd.Execute(out object recordsAffected, Type.Missing, -1);
                        entryReset();
                        con.Close();
                        loaddata();
                        MessageBox.Show("Note Deleted.", "Delete", MessageBoxButtons.OK);
                        
                    }

                }
                else
                {
                    MessageBox.Show("Nothing selected", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    con.Close() ;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
