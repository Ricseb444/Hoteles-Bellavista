using System;
using System.Drawing;
using System.Windows.Forms;

namespace UIL
{
    public partial class FrmMainMenu : Form
    {
        public FrmMainMenu()
        {
            InitializeComponent();
        }

        public void Desactivarpnl()
        {
            pnlHuespedes.Visible = false;
            pnlHabitaciones.Visible = false;
            pnlMobiliario.Visible = false;
            pnlReservaciones.Visible = false;
            pnlHoteles.Visible = false;
            pnlHistorial.Visible = false;
        }

        private void btnHuespedes_MouseEnter(object sender, EventArgs e)
        {
            pnlHuespedes.Visible = true;
        }

        private void btnHabitaciones_MouseEnter(object sender, EventArgs e)
        {
            pnlHabitaciones.Visible = true;
        }

        private void btnMobiliario_MouseEnter(object sender, EventArgs e)
        {
            pnlMobiliario.Visible = true;
        }

        private void btnReservaciones_MouseEnter(object sender, EventArgs e)
        {
            pnlReservaciones.Visible = true;
        }

        private void btnHoteles_MouseEnter(object sender, EventArgs e)
        {
            pnlHoteles.Visible = true;
        }

        private void btnHistorial_MouseEnter(object sender, EventArgs e)
        {
            pnlHistorial.Visible = true;
        }

        private void pnlHuespedes_MouseEnter(object sender, EventArgs e)
        {
            pnlHuespedes.Visible = true;
        }

        private void pnlRojo_MouseEnter(object sender, EventArgs e)
        {
            Desactivarpnl();
        }

        private void pnlLemaImg_MouseEnter(object sender, EventArgs e)
        {
            Desactivarpnl();
        }

        private void picPiscina_MouseEnter(object sender, EventArgs e)
        {
            Desactivarpnl();
        }        

        private void lblLema_MouseEnter(object sender, EventArgs e)
        {
            Desactivarpnl();
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == "Buscar huésped...")
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.White;
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (txtBuscar.Text == "")
            {
                txtBuscar.Text = "Buscar huésped...";
                txtBuscar.ForeColor = Color.DarkGray;
            }
        }

        private void btnRegHuesped_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnRegHuesped";
            ventanaCRUD.ShowDialog();

        }

        private void btnModRegis_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnModRegis";
            ventanaCRUD.ShowDialog();
        }

        private void btnRegHabit_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnRegHabit";
            ventanaCRUD.ShowDialog();
        }

        private void btnModHabit_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnModHabit";
            ventanaCRUD.ShowDialog();
        }

        private void btnRegMobil_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnRegMobil";
            ventanaCRUD.ShowDialog();
        }

        private void btnModMobil_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnModMobil";
            ventanaCRUD.ShowDialog();
        }

        private void btnRegReser_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnRegReser";
            ventanaCRUD.ShowDialog();
        }

        private void btnModReser_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnModReser";
            ventanaCRUD.ShowDialog();
        }

        private void btnRegHotel_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnRegHotel";
            ventanaCRUD.ShowDialog();
        }

        private void btnModHotel_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnModHotel";
            ventanaCRUD.ShowDialog();
        }

        private void btnShowRegis_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnShowRegis";
            ventanaCRUD.ShowDialog();
        }

        private void btnShowHabit_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnShowHabit";
            ventanaCRUD.ShowDialog();
        }

        private void btnShowMobil_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnShowMobil";
            ventanaCRUD.ShowDialog();
        }

        private void btnShowReser_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnShowReser";
            ventanaCRUD.ShowDialog();
        }

        private void btnShowHotel_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnShowHotel";
            ventanaCRUD.ShowDialog();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text != "Buscar huésped..." && txtBuscar.Text != "")
            {
                CRUD ventanaCRUD = new CRUD();
                ventanaCRUD.BotonPresionado = "btnModRegis";
                ventanaCRUD.Vartxt1 = txtBuscar.Text;
                ventanaCRUD.ShowDialog();
                txtBuscar.Text = "Buscar huésped...";
                txtBuscar.ForeColor = Color.DarkGray;
            }
            
        }

        private void btnConta_Click(object sender, EventArgs e)
        {
            CRUD ventanaCRUD = new CRUD();
            ventanaCRUD.BotonPresionado = "btnConta";
            ventanaCRUD.Vartxt1 = txtBuscar.Text;
            ventanaCRUD.ShowDialog();
        }
    }
}
