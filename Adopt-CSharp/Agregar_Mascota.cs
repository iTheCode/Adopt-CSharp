﻿using System;
using System.Drawing;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Globalization;
using System.Drawing.Imaging;

namespace Adopt_CSharp
{
    public partial class Agregar_Mascota : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        private int id_usuario;
        private string image64;
        int categoria;

        public Agregar_Mascota(int id_usuario)
        {
            this.id_usuario = id_usuario;
            InitializeComponent();
            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
        }
        BaseDeDatos am = new BaseDeDatos();
        private string imagen;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            string nombre = am.selectstring("select nombre from login where id_usuario = '" + this.id_usuario + "'");
            string apellido = am.selectstring("select apellido from login where id_usuario = '" + this.id_usuario + "'");
            string image = am.selectstring("select img from login where id_usuario = '" + this.id_usuario + "'");
            Panel p = new Panel(this.id_usuario);
            p.pictureBox36.Image = Base64ToImage(image);
            p.materialLabel16.Text = nombre + " " + apellido;
            p.Show();
        }

        private void materialSingleLineTextField4_Click(object sender, EventArgs e)
        {

        }
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public string ImageToBase64(Image image, ImageFormat format)
        {
            //Convertir Imagen a Base 64
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    imagen = openFileDialog2.FileName;
                    pictureBox4.Image = Image.FromFile(imagen);
                    pictureBox5.Image = Image.FromFile(imagen);

                    ImageFormat thisFormat = pictureBox4.Image.RawFormat;

                    image64 = ImageToBase64(pictureBox4.Image, thisFormat);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
                {
                switch(btn.Name)
                   {
                    case "materialRadioButton1": categoria = 1;
                         break;
                    case "materialRadioButton2": categoria = 2;
                        break;
                    case "materialRadioButton3": categoria = 3;
                        break;
                    case "materialRadioButton4": categoria = 4;
                        break;
                    }
                }
            //Acá insertas al sql todo
            if (string.IsNullOrEmpty(txtnombre.Text))
            {
                MessageBox.Show("Debe completar el nombre");
                return;
            }
            if (string.IsNullOrEmpty(txtraza.Text))
            {
                MessageBox.Show("Debe completar el raza");
                return;
            }
            if (string.IsNullOrEmpty(txtedad.Text))
            {
                MessageBox.Show("Debe completar la edad");
                return;
            }
            if (string.IsNullOrEmpty(txthistoria.Text))
            {
                MessageBox.Show("Debe completar la historia");
                return;
            }
            if (string.IsNullOrEmpty(txtubicacion.Text))
            {
                MessageBox.Show("Debe completar la ubicacion");
                return;
            }
            else
            {
                string agregar = "insert into animales (id_cliente,nombre,raza,edad,tipo,informacion,ubicacion,img) values ('" + this.id_usuario + "','" + txtnombre.Text + "','" + txtraza.Text + "'," + txtedad.Text + ",'" + categoria + "','" + txthistoria.Text + "','" + txtubicacion.Text + "', '" + image64 + "')";
                if (am.executecommand(agregar))
                {
                    int id_animales = Int32.Parse(am.selectstring("select id_animales from animales where id_cliente = '" + this.id_usuario + "' and nombre = '" + txtnombre.Text + "' and raza = '" + txtraza.Text + "' and edad = " + txtedad.Text + " and tipo = '" + categoria + "' and informacion = '" + txthistoria.Text + "' and ubicacion = '" + txtubicacion.Text + "' and img = '" + image64 + "'"));
                        MessageBox.Show("El animal se ha puesto en adopción.");
                        Perfil_Mascota p = new Perfil_Mascota(this.id_usuario, id_animales);
                        p.lblnombre.Text = txtnombre.Text;
                        p.lblraza.Text = txtraza.Text;
                        p.lbledad.Text = txtedad.Text;
                        p.lblcategoria.Text = Convert.ToString(categoria);
                        p.lblubicacion.Text = txtubicacion.Text;
                        p.pictureBox4.Image = Image.FromFile(imagen);
                        p.pictureBox5.Image = Image.FromFile(imagen);
                        p.Show();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar correctamente el animal.");
                }
                this.Hide();
            }

        }
        
        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }

    }
}
