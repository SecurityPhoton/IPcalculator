using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IPCalculator
{
    public partial class Form1 : Form
    {
        public struct IPaddress
        {
            public int fo1;
            public int fo2;
            public int fo3;
            public int fo4;
        }

        public IPaddress IPaddr, Mask, Net, FH, LH, BC;
        int sum1;

        public string tobin(int a)
        {
            string result;
            result = "";
            int[] octet = new int[8];
            int tmp1, tmp2;
            tmp1 = a;
            for (int i = 0; i < 8; i++)
            {
                tmp2 = tmp1 / 2;
                octet[i] = tmp1 % 2;
                tmp1 = tmp2;
            }
            Array.Reverse(octet);

            result = Convert.ToString(octet[0]) + Convert.ToString(octet[1]) + Convert.ToString(octet[2]) + Convert.ToString(octet[3]) + Convert.ToString(octet[4]) + Convert.ToString(octet[5]) + Convert.ToString(octet[6]) + Convert.ToString(octet[7]);
            return result;
        }

        public string wildcard(int a)
        {
            string result;
            result = "";
            int[] octet = new int[8];
            int tmp1, tmp2;
            tmp1 = a;
            for (int i = 0; i < 8; i++)
            {
                tmp2 = tmp1 / 2;
                octet[i] = tmp1 % 2;
                tmp1 = tmp2;
            }
            Array.Reverse(octet);
            for (int j = 0; j < 8; j++) { if (octet[j] == 1) octet[j] = 0; else octet[j] = 1; }

            result = Convert.ToString(octet[0]) + Convert.ToString(octet[1]) + Convert.ToString(octet[2]) + Convert.ToString(octet[3]) + Convert.ToString(octet[4]) + Convert.ToString(octet[5]) + Convert.ToString(octet[6]) + Convert.ToString(octet[7]);
            return result;
        }

        public int bincount(int a)
        {
            int one;
            one = 0;

            int[] octet = new int[8];
            int tmp1, tmp2;
            tmp1 = a;
            for (int i = 0; i < 8; i++)
            {
                tmp2 = tmp1 / 2;
                octet[i] = tmp1 % 2;
                tmp1 = tmp2;
                if (octet[i] == 1) one++;
            }

            return one;
        }

        public void Calc(IPaddress IPaddr,IPaddress Mask)
        {
            
            //MessageBox.Show(tobin(128));

            try
            {

                
                if ((IPaddr.fo1 > 255) || (IPaddr.fo2 > 255) || (IPaddr.fo3 > 255) || (IPaddr.fo4 > 255) || (Mask.fo1 > 255) || (Mask.fo2 > 255) || (Mask.fo3 > 255) || (Mask.fo4 > 255))
                { MessageBox.Show("Adress octet can't be more then 255 !!!"); return; }

                Net.fo1 = (IPaddr.fo1 & Mask.fo1);
                Net.fo2 = (IPaddr.fo2 & Mask.fo2);
                Net.fo3 = (IPaddr.fo3 & Mask.fo3);
                Net.fo4 = (IPaddr.fo4 & Mask.fo4);

                BC.fo1 = Net.fo1 | (255 - Mask.fo1); //MessageBox.Show(Convert.ToString(Mask.fo3));
                BC.fo2 = Net.fo2 | (255 - Mask.fo2);
                BC.fo3 = Net.fo3 | (255 - Mask.fo3);
                BC.fo4 = Net.fo4 | (255 - Mask.fo4);

                FH.fo1 = Net.fo1;
                FH.fo2 = Net.fo2;
                FH.fo3 = Net.fo3;
                FH.fo4 = Net.fo4 + 1;

                LH.fo1 = BC.fo1;
                LH.fo2 = BC.fo2;
                LH.fo3 = BC.fo3;
                LH.fo4 = BC.fo4 - 1;


                //  textBox10.Text= Convert.ToBase64String (Convert.ToSByte(Mask.fo1));
                //  textBox10.Text = Convert.ToString(192 ^ 2);
                sum1 = bincount(Mask.fo1) + bincount(Mask.fo2) + bincount(Mask.fo3) + bincount(Mask.fo4);// MessageBox.Show(Convert.ToString(sum1));
                label1.Text = "/" + Convert.ToString(sum1);

                textBox1.AppendText(tobin(IPaddr.fo1) + ' ' + tobin(IPaddr.fo2) + ' ' + tobin(IPaddr.fo3) + ' ' + tobin(IPaddr.fo4)+" ip addres\n");
                textBox1.AppendText(tobin(Mask.fo1) + ' ' + tobin(Mask.fo2) + ' ' + tobin(Mask.fo3) + ' ' + tobin(Mask.fo4) + " mask\n");
                textBox1.AppendText(Convert.ToString(Net.fo1) + '.' + Convert.ToString(Net.fo2) + '.' + Convert.ToString(Net.fo3) + '.' + Convert.ToString(Net.fo4)+" net\n"); // net
                textBox1.AppendText(Convert.ToString(BC.fo1) + '.' + Convert.ToString(BC.fo2) + '.' + Convert.ToString(BC.fo3) + '.' + Convert.ToString(BC.fo4)+" broadcast\n"); // broadcast
                textBox1.AppendText(Convert.ToString(FH.fo1) + '.' + Convert.ToString(FH.fo2) + '.' + Convert.ToString(FH.fo3) + '.' + Convert.ToString(FH.fo4)+" first host\n"); // first host
                textBox1.AppendText(Convert.ToString(LH.fo1) + '.' + Convert.ToString(LH.fo2) + '.' + Convert.ToString(LH.fo3) + '.' + Convert.ToString(LH.fo4)+" last host\n"); // last host
                textBox1.AppendText(Convert.ToString(System.Math.Pow(2, (32 - sum1)) - 2)+" # of hosts\n"); 
                if (System.Math.Pow(2, (32 - sum1)) - 2 == -1) textBox1.AppendText("0\n");// # of hosts
                textBox1.AppendText(Convert.ToString(System.Math.Pow(2, (sum1)) - 2)+"# of nets\n"); // # of nets
                textBox1.AppendText(wildcard(Mask.fo1) + ' ' + wildcard(Mask.fo2) + ' ' + wildcard(Mask.fo3) + ' ' + wildcard(Mask.fo4)+" wildcard\n");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        public bool iscorrectIP(string ip)
        {
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            return Regex.IsMatch(ip, pattern); // returns true
        }

        public IPaddress ToOctets(string ip)
        {
            IPaddress ipaddres;
            string[] octets = ip.Split('.',',');
            ipaddres.fo1 = Convert.ToInt32(octets[0]);
            ipaddres.fo2 = Convert.ToInt32(octets[1]);
            ipaddres.fo3 = Convert.ToInt32(octets[2]);
            ipaddres.fo4 = Convert.ToInt32(octets[3]);

            return ipaddres;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            maskedTextBox1.ResetOnSpace = false;
            

            if (iscorrectIP(maskedTextBox1.Text) && iscorrectIP(comboBox1.Text))
            {
               IPaddr= ToOctets(maskedTextBox1.Text);
               Mask = ToOctets(comboBox1.Text);

               Calc(IPaddr,Mask);
            }
        }
    }
}
