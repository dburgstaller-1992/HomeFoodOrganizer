using Microsoft.VisualBasic;

namespace Essensausgleich
{
    //Why does it say "Form1" - give it a speaking name. (e.g. of your "MainForm" or your application name. )
    public partial class Form1 : Form
    {
        //language englisch

        //why passing a "null" object to the constructor of "Bewohner"? 
        private Bewohner bewohner1 = new(null);
        private Bewohner bewohner2 = new(null);
        public Form1()
        {
            InitializeComponent();
            LblToolStrip.Text = "";
        }// Try to have a empty line between methods. And add method descriptions - especially for public methods.
        public void WriteLineS(string s) //Why was this extracted?  It's a single line. There is no benefit in doing so. 
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            decimal Endwert = 0;
            string? zBezahlender; // this is a followup error on the null of the constructor of the Bewohner class
            if (bewohner1.name != null && bewohner2.name != null)
            {
                Endwert = (bewohner1.Ausgaben + bewohner2.Ausgaben) / 2;
                if (bewohner1.Ausgaben > 0 || bewohner2.Ausgaben > 0)
                {
                    if (bewohner1.Ausgaben > bewohner2.Ausgaben)
                    {
                        Endwert = bewohner1.Ausgaben - Endwert;
                        zBezahlender = bewohner2.name;
                        LblBill.Text = Convert.ToString(Endwert);
                        LblZuBezahlender.Text = zBezahlender;
                    }
                    else
                    {
                        Endwert = bewohner2.Ausgaben - Endwert;
                        zBezahlender = bewohner1.name;
                        LblBill.Text = Convert.ToString(Endwert);
                        LblZuBezahlender.Text = zBezahlender;
                    }
                }
                else LblToolStrip.Text = $"Mindestens eine Partei muss Ausgaben hinterlegen";
            }
            else LblToolStrip.Text = $"Es wurden nicht mindestens 2 User Angelegt";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            AddUser(); // why abstract it? it's just a click handler calling one single method. 
        }
        private void AddUser()
        {
            if (txtBoxAddUser.Text != "") // there is a better method to do this. This is called escape clauses - the clauses in which you don't want the method to do anything
            {

                // if txtBoxAddUser.Text == "") { 
                    //return; 
                // }
                if (bewohner1.name == null || bewohner2.name == null) // there should be a better way to distinguish which user to take. 
                // you could also argue, that you don't want to be limited to 2 users? 
                {
                    if (bewohner1.name == null)
                    {
                        bewohner1.name = txtBoxAddUser.Text;
                        LblBewohner1.Text = bewohner1.name;
                        cBoxUser.Items.Add(txtBoxAddUser.Text);
                        cBoxUser.SelectedIndex = cBoxUser.Items.Count - 1;
                        LblToolStrip.Text = $"Bewohner {bewohner1.name} wurde angelegt";
                    }
                    else if (bewohner2.name == null && txtBoxAddUser.Text != bewohner1.name)
                    {
                        bewohner2.name = txtBoxAddUser.Text;
                        LblBewohner2.Text = bewohner2.name;
                        cBoxUser.Items.Add(txtBoxAddUser.Text);
                        cBoxUser.SelectedIndex = cBoxUser.Items.Count - 1;
                        LblToolStrip.Text = $"Bewohner {bewohner2.name} wurde angelegt";
                    }
                    else LblToolStrip.Text = $"Name gleich wie User1 bitte anderen waehlen"; // if using else always use { } - because otherwise the else branch can be overseen easily. Also folding of code is improved. 
                }
                else LblToolStrip.Text = $"Maximale User anzahl bereits Angelegt";
            }
            else LblToolStrip.Text = $"Kein User Name eingegeben";
        }
        private void BtnAddBill_Click(object sender, EventArgs e)
        {
            if (cBoxUser.Text != "")
            {
                decimal bill = 0;
                try
                {

                    bill = Convert.ToDecimal(txtBoxAddBill.Text); // you could use Decimal.tryParse
                   
                   // you could just bind the cBoxUser to a "bewohner" object - which will make your life easier
                    if (bewohner1.name == cBoxUser.Text && cBoxUser.Text != "")
                    {
                        bewohner1.AddBetrag(txtBoxCategorie.Text, bill);
                        LblTotalAmountBew1.Text = Convert.ToString(bewohner1.Ausgaben);
                        LblToolStrip.Text = $"Betrag {bill} der Kategorie {txtBoxCategorie.Text} hinzugefuegt";
                    }
                    else if (bewohner2.name == cBoxUser.Text && cBoxUser.Text != "")
                    {
                        bewohner2.AddBetrag(txtBoxCategorie.Text, bill);
                        LblTotalAmountBew2.Text = Convert.ToString(bewohner2.Ausgaben);
                        LblToolStrip.Text = $"Betrag {bill} der Kategorie {txtBoxCategorie.Text} hinzugefuegt";
                    }
                    else
                    {
                        LblToolStrip.Text = $"Error keine Bewohner wurde mit der im Dropdown ausgewaehlten User identifiziert";
                    }
                }
                catch (Exception exectionMsg)
                {

                    LblToolStrip.Text = exectionMsg.Message;
                }
            }
            else LblToolStrip.Text = $"Missing Username";
        }
        public void BtnAuflisten_Click(object sender, EventArgs e)
        {
            if (cBoxUser.Text != "")
            {
                if (bewohner1.name == cBoxUser.Text)
                {
                    beitragsauflistungForm beitragsauflistung = new(this);
                    beitragsauflistung.FillDataGrid(bewohner1.BewohnerAusgabenListe());
                    beitragsauflistung.ShowDialog();
                }
                else if (bewohner2.name == cBoxUser.Text)
                {
                    beitragsauflistungForm beitragsauflistung = new(this);
                    beitragsauflistung.FillDataGrid(bewohner2.BewohnerAusgabenListe());
                    beitragsauflistung.ShowDialog();
                }
                else
                {
                    LblToolStrip.Text = $"Error keine Bewohner wurde mit der im Dropdown ausgewaehlten User identifiziert";
                }
            }
            else LblToolStrip.Text = $"Kein User Vorhanden bzw Ausgewaehlt";
        }
        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm settingsForm = new();
            settingsForm.ShowDialog();
        }
        private void SaveFileXML_Click(object sender, EventArgs e) // data operations should not be within the main class. 
        // create a seperate File e.g. XMLPersistence.cs
        /**
            XMLPersisence xmlPersistence = new XMLPersistence();
            xmlPersistence.save(.....);
        **/
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                settings.ConformanceLevel = ConformanceLevel.Auto;

                XmlWriter writer = XmlWriter.Create("abrechnung.xml", settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("Abrechnung");

                foreach (var Betrag in bewohner1.BewohnerAusgabenListe())
                {
                    writer.WriteStartElement("a");
                    writer.WriteAttributeString("BewohnerName", bewohner1.BewohnerName());
                    writer.WriteAttributeString("kategorie", Betrag.kategorie);
                    writer.WriteAttributeString("Betrag", Betrag.wert.ToString());
                    writer.WriteEndElement();

                }
                foreach (var Betrag in bewohner2.BewohnerAusgabenListe())
                {
                    writer.WriteStartElement("b");
                    writer.WriteAttributeString("BewohnerName", bewohner2.BewohnerName());
                    writer.WriteAttributeString("kategorie", Betrag.kategorie);
                    writer.WriteAttributeString("Betrag", Betrag.wert.ToString());
                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
                writer.Close();
            }
            catch (Exception writerException)
            {
                WriteLineS(writerException.Message);
                LblToolStrip.Text = writerException.Message;

            }

        }
        private void OfdXML_Click(object sender, EventArgs e) // should also be extracted to the XMLPersistence.cs file and just return List<Bewohner> which you just need to bind here
        {
            if (!File.Exists("abrechnung.xml")) // "Magic numbers" - you could use a constant on top. 
            {
                LblToolStrip.Text = "File not found";
                return;
            }
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings(); 
                settings.Async = true;

                XmlReader reader = XmlReader.Create("abrechnung.xml", settings); // repeated constant here
                List<Betrag> LoadListB1 = new List<Betrag>();
                List<Betrag> LoadListB2 = new List<Betrag>();
                string b1Name = "";
                string b2Name = "";
                decimal b1betrag = 0;
                decimal b2betrag = 0;
                string kat1 = "";
                string kat2 = "";
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name != "Abrechnung")
                        {
                            if (reader.Name == "a")
                            {
                                if (reader.AttributeCount > 0)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name != "a")
                                            switch (reader.Name)
                                            {
                                                case "BewohnerName":
                                                    b1Name = reader.Value;
                                                    break;
                                                case "kategorie":
                                                    kat1 = reader.Value;
                                                    break;
                                                case "Betrag":
                                                    b1betrag = Convert.ToDecimal(reader.Value);
                                                    break;
                                            }
                                    }
                                    LoadListB1.Add(new Betrag(kat1, b1betrag));
                                }
                            }
                            if (reader.Name == "b")
                            {
                                if (reader.AttributeCount > 0)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name != "b")
                                            switch (reader.Name)
                                            {
                                                case "BewohnerName":
                                                    b2Name = reader.Value;
                                                    break;
                                                case "kategorie":
                                                    kat2 = reader.Value;
                                                    break;
                                                case "Betrag":
                                                    b2betrag = Convert.ToDecimal(reader.Value);
                                                    break;
                                            }
                                    }
                                    LoadListB2.Add(new Betrag(kat2, b2betrag));
                                }
                            }
                        }
                    }
                }
                reader.Close();
                //Formular refresh
                bewohner1.LoadBewohnerDataXML(b1Name, LoadListB1);
                bewohner2.LoadBewohnerDataXML(b2Name, LoadListB2);
                LblBewohner1.Text = bewohner1.name;
                LblBewohner2.Text = bewohner2.name;
                bewohner1.RefreshBetrag();
                bewohner2.RefreshBetrag();
                LblTotalAmountBew1.Text = bewohner1.Ausgaben.ToString();
                LblTotalAmountBew2.Text = bewohner2.Ausgaben.ToString();
                cBoxUser.Items.Add(bewohner1.GetBewohnerName());
                cBoxUser.Items.Add(bewohner2.GetBewohnerName());
                cBoxUser.SelectedIndex = cBoxUser.Items.Count - 1;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) // what is "newToolStripMenuItem"? 
        {
            bewohner1.ResetBewohnerData();
            bewohner2.ResetBewohnerData();
            LblBewohner1.Text = "Bew1";
            LblBewohner2.Text = "Bew2";
            LblBill.Text = "0";
            LblTotalAmountBew1.Text = "0";
            LblTotalAmountBew2.Text = "0";
            cBoxUser.Items.Clear();
            cBoxUser.Text = "";
        }
    }
}
