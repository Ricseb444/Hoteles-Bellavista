using Hoteles.BLL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UIL
{
    public partial class ReservacionesForm : Form
    {


        private string botonPresionado;
        private string vartxt1;

        string[] fpago = { "Efectivo", "Tarjeta", "Mixto" };
        public string BotonPresionado
        {
            get { return botonPresionado; }
            set { botonPresionado = value; }
        }
        public string Vartxt1
        {
            get { return vartxt1; }
            set { vartxt1 = value; }
        }

        private void RellenarCombos()
        {
            foreach (string item in fpago)
            {
                combo6.Items.Add(item);
            }
        }

        public ReservacionesForm()
        {
            InitializeComponent();
            RellenarCombos();
        }

        private void ReservacionesForm_Load(object sender, EventArgs e)
        {
            txt1.Text = Vartxt1;                        
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            Reservacion nuevaReservacion = new Reservacion();
            if (!string.IsNullOrEmpty(txt1.Text) &&
                !string.IsNullOrEmpty(txt2.Text) &&
                !string.IsNullOrEmpty(txt3.Text) &&
                !string.IsNullOrEmpty(txt7.Text) &&
                combo6.SelectedIndex != -1)
            {
                // Asignar los valores de los campos de texto a la nueva reservación
                nuevaReservacion.IdCliente = Convert.ToInt32(txt1.Text);
                nuevaReservacion.CodHotel = Convert.ToInt32(txt2.Text);
                nuevaReservacion.CodHabit = txt3.Text;
                nuevaReservacion.FechaLlegada = dateTimePicker1.Value;
                nuevaReservacion.FechaSalida = dateTimePicker2.Value;
                nuevaReservacion.FormaPago = fpago[combo6.SelectedIndex];
                nuevaReservacion.Voucher = txt7.Text;
                switch (combo6.SelectedIndex)
                {
                    case 0: // Efectivo
                        nuevaReservacion.MontoTarjeta = 0;
                        nuevaReservacion.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                        break;
                    case 1: // Tarjeta
                        nuevaReservacion.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                        nuevaReservacion.MontoEfectivo = 0;
                        break;
                    case 2: // Mixto
                        nuevaReservacion.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                        nuevaReservacion.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Debe completar todos los campos");
                return;
            }
            // Crear una nueva instancia de ReservacionBLL
            ReservacionBLL reservacionBLL = new ReservacionBLL();

            // Crear la nueva reserva
            ResultadoOperacion resultado = reservacionBLL.CrearReserva(nuevaReservacion);

            if (resultado.Exito)
            {
                MessageBox.Show($"Reservación registrada con éxito.");
            }
            else
            {
                MessageBox.Show($"Hubo un error al registrar la reservación: {resultado.Mensaje}");
                return;
            }

        }

        private void combo6_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (combo6.SelectedIndex)
            {
                case 0:
                    {
                        panel7.Visible = false;
                        panel8.Visible = false;
                        panel9.Visible = true;
                        break;
                    }
                case 1:
                    {
                        panel7.Visible = true;
                        panel8.Visible = true;
                        panel9.Visible = false;
                        break;
                    }
                case 2:
                    {
                        panel9.Visible = true;
                        break;
                    }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            combo6.SelectedIndex = -1;
            txt7.Text = "";
            txt8.Text = "";
            txt9.Text = "";
        }
    }
}
