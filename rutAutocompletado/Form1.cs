using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rutAutocompletado
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<char> listRut = new List<char>();
        private void txtRut_KeyPress(object sender, KeyPressEventArgs e)
        {
            int seleccion = txtRut.SelectionStart;
            label1.Text = "";

            if (!Char.IsControl(e.KeyChar))
            {
                if (Char.IsDigit(e.KeyChar) || e.KeyChar == 'k' || e.KeyChar == 'K')
                {
                    int longitud = listRut.Count;
                    e.Handled = true;

                    if (longitud >= 12)
                    {
                        label1.Text = "Espacio completo";
                    }
                    else
                    {
                        insertarCaracter(seleccion, e.KeyChar);
                    }                   
                }
                else
                {
                    e.Handled = true;
                }

            }

        }

        void insertarCaracter(int seleccion, char caracter)
        {
            listRut.Clear();
            for (int i = 0; i < txtRut.Text.Length; i++)
            {
                listRut.Add(txtRut.Text[i]);
            }

            //MessageBox.Show(seleccion.ToString());

            listRut.Insert(seleccion, caracter);
            seleccion++;

            while (listRut.IndexOf('-') >= 0)
            {
                seleccion--;
                listRut.Remove('-');
            }
            while (listRut.IndexOf('.') >= 0)
            {
                seleccion--;
                listRut.Remove('.');
            }

            if (listRut.Count > 1)
            {
                seleccion++;
                listRut.Insert(listRut.Count - 1, '-');
            }

            if (listRut.Count > 5)
            {
                seleccion++;
                listRut.Insert(listRut.Count - 5, '.');
            }

            if (listRut.Count > 9)
            {
                seleccion++;
                listRut.Insert(listRut.Count - 9, '.');
            }

            txtRut.Text = String.Join("", listRut.ToArray());
            txtRut.SelectionStart = seleccion;
        }

        private void txtRut_KeyUp(object sender, KeyEventArgs e)
        {         
            int seleccion = txtRut.SelectionStart;
            if (e.KeyValue == 8 || e.KeyValue == 46)
            {
                label1.Text = "";
                listRut.Clear();
                if (txtRut.TextLength > 0)
                {
                    eliminarCaracter(seleccion);
                }
            }
        }

        void eliminarCaracter(int seleccion)
        {
            for (int i = 0; i < txtRut.Text.Length; i++)
            {
                if (Char.IsNumber(txtRut.Text[i]) || txtRut.Text[i] == 'k' || txtRut.Text[i] == 'K')
                {
                    listRut.Add(txtRut.Text[i]);
                }
                else
                {
                    seleccion--;
                }
            }

            if (listRut.Count > 1)
            {
                listRut.Insert(listRut.Count - 1, '-');
                seleccion++;
            }

            if (listRut.Count > 5)
            {
                listRut.Insert(listRut.Count - 5, '.');
                seleccion++;
            }

            if (listRut.Count > 9)
            {
                listRut.Insert(listRut.Count - 9, '.');
                seleccion++;
            }

            txtRut.Text = String.Join("", listRut.ToArray());
            while (seleccion <= -1)
            {
                seleccion++;
            }
            txtRut.SelectionStart = seleccion;
        }


        bool validarRut(string rut)
        {
            bool valido = false;
            int[] constantes = { 3, 2, 7, 6, 5, 4, 3, 2 };

            while (rut.Length < 10)
            {
                rut = "0" + rut;
            }

            double suma = 0;
            for (int i = 0; i < 8; i++)
            {
                int valorActual = int.Parse(rut[i].ToString());
                suma += constantes[i] * valorActual;
            }
            double division = suma / 11.0;
            double decimales = division - (int)division;
            double digitoNumerico = Math.Round(11 - (11 * decimales));
            char digito;
            if (digitoNumerico == 11)
            {
                digito = '0';
            }
            else if (digitoNumerico == 10)
            {
                digito = 'k';
            }
            else
            {
                digito = Convert.ToChar(digitoNumerico.ToString());
            }
        

            if (digito == rut[9])
            {
                valido = true;
            }
            else if (digito == 'k' && rut[9] == 'K')
            {
                valido = true;
            }
            else
            {
                valido = false;
            }
            return valido;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtRut.Text.Contains('-'))
            {
                string rut = "";
                foreach (char c in txtRut.Text)
                {
                    if (c != '.')
                    {
                        rut += c;
                    }
                }

                bool correcto = validarRut(rut);
                label1.Text = correcto ? "Rut correcto" : "Rut invalido";
            }
        }
    }
}
