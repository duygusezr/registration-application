using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Form_Uygulaması
{
    public partial class Form1 : Form
    {
        // İl ve ilçeler sözlüğü
        private Dictionary<string, List<string>> ilVeIlceler = new Dictionary<string, List<string>>()
        {
            { "İstanbul", new List<string> { "Kadıköy", "Beşiktaş", "Üsküdar", "Şişli", "Bakırköy", "Fatih", "Sarıyer", "Eyüp", "Zeytinburnu", "Kartal" } },
            { "Ankara", new List<string> { "Çankaya", "Keçiören", "Mamak", "Yenimahalle", "Altındağ", "Gölbaşı", "Sincan", "Etimesgut", "Pursaklar", "Ayaş" } },
            { "İzmir", new List<string> { "Konak", "Karşıyaka", "Bornova", "Buca", "Gaziemir", "Çiğli", "Narlıdere", "Balçova", "Bayraklı", "Foça" } },
            { "Bursa", new List<string> { "Osmangazi", "Nilüfer", "Yıldırım", "İnegöl", "Gemlik", "Gürsu", "Kestel", "Mudanya", "Orhangazi", "Karacabey" } },
            { "Antalya", new List<string> { "Muratpaşa", "Kepez", "Konyaaltı", "Alanya", "Manavgat", "Kemer", "Serik", "Kumluca", "Finike", "Kaş" } },
            { "Adana", new List<string> { "Seyhan", "Yüreğir", "Çukurova", "Sarıçam", "Ceyhan", "Kozan", "Feke", "Aladağ", "Karaisalı", "Pozantı" } },
            { "Konya", new List<string> { "Selçuklu", "Meram", "Karatay", "Ereğli", "Akşehir", "Beyşehir", "Çumra", "Seydişehir", "Ilgın", "Kadınhanı" } },
            { "Kayseri", new List<string> { "Kocasinan", "Melikgazi", "Talas", "Hacılar", "Develi", "Yeşilhisar", "Pınarbaşı", "Bünyan", "İncesu", "Tomarza" } },
            { "Gaziantep", new List<string> { "Şahinbey", "Şehitkamil", "Nizip", "Islahiye", "Oğuzeli", "Araban", "Yavuzeli", "Karkamış", "Nurdağı" } },
            { "Mersin", new List<string> { "Akdeniz", "Yenişehir", "Mezitli", "Toroslar", "Tarsus", "Erdemli", "Silifke", "Mut", "Anamur", "Aydıncık" } },
            { "Samsun", new List<string> { "Atakum", "İlkadım", "Canik", "Bafra", "Çarşamba", "Vezirköprü", "Terme", "Havza", "Alaçam", "19 Mayıs" } },
            { "Kocaeli", new List<string> { "İzmit", "Gebze", "Darıca", "Çayırova", "Derince", "Başiskele", "Körfez", "Gölcük", "Kartepe", "Kandıra" } },
            { "Eskişehir", new List<string> { "Odunpazarı", "Tepebaşı", "Sivrihisar", "Çifteler", "Seyitgazi", "Beylikova", "Mihalıççık", "İnönü", "Sarıcakaya", "Alpu" } },
            { "Diyarbakır", new List<string> { "Bağlar", "Kayapınar", "Sur", "Yenişehir", "Ergani", "Bismil", "Silvan", "Çermik", "Çınar", "Kulp" } },
            { "Hatay", new List<string> { "Antakya", "İskenderun", "Defne", "Samandağ", "Kırıkhan", "Reyhanlı", "Dörtyol", "Erzin", "Belen", "Altınözü" } },
            { "Trabzon", new List<string> { "Ortahisar", "Akçaabat", "Yomra", "Of", "Vakfıkebir", "Beşikdüzü", "Maçka", "Araklı", "Sürmene", "Çarşıbaşı" } },
            { "Malatya", new List<string> { "Yeşilyurt", "Battalgazi", "Doğanşehir", "Akçadağ", "Pütürge", "Hekimhan", "Darende", "Arguvan", "Yazıhan", "Kuluncak" } },
            { "Manisa", new List<string> { "Yunusemre", "Şehzadeler", "Turgutlu", "Akhisar", "Salihli", "Soma", "Kırkağaç", "Alaşehir", "Saruhanlı", "Kula" } },
            { "Balıkesir", new List<string> { "Karesi", "Altıeylül", "Edremit", "Bandırma", "Ayvalık", "Burhaniye", "Erdek", "Gönen", "Bigadiç", "Sındırgı" } }
        };

        // Bağlantı dizesi
        private string connectionString = "Data Source=.;Initial Catalog=Form_Application;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            // İlk ComboBox'a iller ekleniyor
            comboBox1.Items.AddRange(ilVeIlceler.Keys.ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // İl seçimine bağlı olarak ilçeler güncelleniyor
            if (comboBox1.SelectedItem != null)
            {
                string selectedIl = comboBox1.SelectedItem.ToString();
                comboBox2.Items.Clear();
                if (ilVeIlceler.ContainsKey(selectedIl))
                {
                    comboBox2.Items.AddRange(ilVeIlceler[selectedIl].ToArray());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Bilgi derleme
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            string il = comboBox1.SelectedItem?.ToString() ?? "";
            string ilce = comboBox2.SelectedItem?.ToString() ?? "";
            string cinsiyet = radioButton1.Checked ? "Kız" : (radioButton2.Checked ? "Erkek" : "");
            List<string> diller = new List<string>();

            if (checkBox1.Checked) diller.Add("İngilizce");
            if (checkBox2.Checked) diller.Add("Almanca");
            if (checkBox3.Checked) diller.Add("Fransızca");
            if (checkBox4.Checked) diller.Add("Rusça");

            string dillerStr = diller.Count > 0 ? string.Join(", ", diller) : "Dil bilinmiyor";
            string ogrenciBilgisi = $"{ad} {soyad} - {il}/{ilce} - {cinsiyet} - Bilinen Diller: {dillerStr}";

            // ListBox'a ekleme
            listBox1.Items.Add(ogrenciBilgisi);

            // Veritabanına ekleme
            try

            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Ogrenciler (Ad, Soyad, Il, Ilce, Cinsiyet, BilinenDiller) VALUES (@Ad, @Soyad, @Il, @Ilce, @Cinsiyet, @BilinenDiller)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", ad);
                        command.Parameters.AddWithValue("@Soyad", soyad);
                        command.Parameters.AddWithValue("@Il", il);
                        command.Parameters.AddWithValue("@Ilce", ilce);
                        command.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                        command.Parameters.AddWithValue("@BilinenDiller", dillerStr);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bilgiler başarıyla veritabanına eklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına eklerken bir hata oluştu: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TextBox'lar ve diğer bileşenler sıfırlanıyor
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.Items.Clear();
            listBox1.Items.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
