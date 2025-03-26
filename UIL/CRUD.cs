using BLL;
using Hoteles.BLL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace UIL
{
    public partial class CRUD : Form
    {
        // Variables que se traen desde el Formulario Menu principal
        private string botonPresionado;
        private string vartxt1;
        // Declaración de arreglos para los comboBox
        string[] codigos_telefono = { "1", "500", "501", "502", "503", "504", "505", "506", "507", "508", "509" };
        string[] respuestaBooleana = { "Sí", "No" };
        string[] Categoria = { "Standard", "Junior", "Premier" };
        string[] numeros = { "1", "2", "3", "4", "5" };
        string[] fpago = { "Efectivo", "Tarjeta", "Mixto" };
        // Variables de generales para saber si hubo cambios
        private Cliente clienteGeneral;
        private Habitacion habitacionGeneral;
        private Mobiliario mobiliarioGeneral;
        private Reservacion ReserGeneral;
        private Hotel hotelGeneral;

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

        // Array para guardar los paneles de telefonos
        // y manejar el evento de agregarlos
        private readonly Panel[] paneles = new Panel[2];
        private int PanelIndex = 0;

        public CRUD()
        {
            InitializeComponent();
            ModificarAlIniciar();
            RellenarCombos();
        }

        // Eventos que se tienen que manejar al Cargar el Formulario

        private void ModificarAlIniciar()
        {
            flpBotones.Controls.Clear();
            flptxt.Controls.Clear();
            panel3.Controls.Remove(combo1);
            panel4.Controls.Remove(combo2);
            panel5.Controls.Remove(combo3);
            panel5.Controls.Remove(comboTel1);
            panel6.Controls.Remove(combo4);
            panel7.Controls.Remove(comboTel2);
            panel8.Controls.Remove(combo5);
            panel8.Controls.Remove(comboTel3);
            panel4.Controls.Remove(dateTimePicker1);
            panel5.Controls.Remove(dateTimePicker2);
            panel6.Controls.Remove(combo6);
            paneles[0] = panel7;
            paneles[1] = panel8;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        // Método para rellenar los comboBox
        private void RellenarCombos()
        {
            foreach (string codigo in codigos_telefono)
            {
                comboTel1.Items.Add(codigo);
                comboTel2.Items.Add(codigo);
                comboTel3.Items.Add(codigo);
            }

            foreach (string item in respuestaBooleana)
            {
                combo2.Items.Add(item);
                combo3.Items.Add(item);
                combo4.Items.Add(item);

            }

            foreach (string item in Categoria)
            {
                combo1.Items.Add(item);
            }

            foreach (string item in numeros)
            {
                combo5.Items.Add(item);
            }

            foreach (string item in fpago)
            {
                combo6.Items.Add(item);
            }
        }

        // Métodos para cambiar la apariencia de flpBotones

        private void AgregarBotones()
        {
            flpBotones.Controls.Add(btnBuscar);
            flpBotones.Controls.Add(btnActualizar);
            flpBotones.Controls.Add(btnEliminar);
            flpBotones.Controls.Add(btnRestaurar);
        }

        private void AgregarBotones(Button boton)
        {
            flpBotones.Controls.Add(btnBuscar);
            flpBotones.Controls.Add(btnActualizar);
            flpBotones.Controls.Add(boton);
            flpBotones.Controls.Add(btnEliminar);
            flpBotones.Controls.Add(btnRestaurar);
        }

        private void AgregarBotones(Button botonA, Button botonB)
        {
            flpBotones.Controls.Add(pnlRelleno1);
            flpBotones.Controls.Add(botonA);
            flpBotones.Controls.Add(pnlRelleno2);
            flpBotones.Controls.Add(botonB);
        }

        private void AgregarBotones(Button botonA, Button botonB, Button botonC)
        {
            flpBotones.Controls.Add(pnlRelleno1);
            flpBotones.Controls.Add(botonA);
            flpBotones.Controls.Add(botonC);
            flpBotones.Controls.Add(botonB);
        }

        // Método para los botones que llevan Combos

        private void AgregarCombos()
        {
            panel3.Controls.Remove(txt3);
            panel4.Controls.Remove(txt4);
            panel5.Controls.Remove(txt5);
            panel6.Controls.Remove(txt6);
            panel8.Controls.Remove(txt8);
            panel3.Controls.Add(combo1);
            panel4.Controls.Add(combo2);
            panel5.Controls.Add(combo3);
            panel6.Controls.Add(combo4);
            panel8.Controls.Add(combo5);
        }

        // Método para ponerle formato de colones a los textbox

        private string ColonFormatter(string text1)
        {
            if (double.TryParse(text1, out double monto))
            {
                string montoFormateado = monto.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR"));
                return montoFormateado;
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt4.Focus();
                return null;
            }
        }

        // Manejo de la apariencia del Formulario

        private void CRUD_Load(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnRegHuesped":
                    {
                        this.Text = "Registro de Huéspedes";
                        groupBox1.Text = "Registrar Huésped";
                        btnReservar.Text = "Registrar y Reservar";
                        label7.Text = "Teléfono 2:";
                        label8.Text = "Teléfono 3:";
                        AgregarBotones(btnRegistrar, btnLimpiar, btnReservar);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        panel5.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        flptxt.Controls.Add(panel10);
                        groupBox2.Visible = false;
                        btnCancel3.Visible = true;
                        btnCancel2.Visible = true;
                        btnCancel1.Visible = true;
                        this.Size = MinimumSize;
                        break;
                    }
                case "btnModRegis":
                    {
                        this.Text = "Modificar Huéspedes";
                        groupBox1.Text = "Modificar Huésped";
                        label7.Text = "Teléfono 2:";
                        label8.Text = "Teléfono 3:";
                        groupBox2.Visible = false;
                        this.Size = MinimumSize;
                        AgregarBotones(btnReservar);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel10);
                        panel5.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        panel4.BackColor = Color.FromArgb(167, 47, 45);
                        btnCancel3.Visible = true;
                        btnCancel2.Visible = true;
                        btnCancel1.Visible = true;
                        if (Vartxt1 != null && Vartxt1 != "")
                        {
                            txt1.Text = Vartxt1;
                            btnBuscar_Click(btnBuscar, EventArgs.Empty);
                        }
                        break;
                    }
                case "btnShowRegis":
                    {
                        this.Text = "Mostrar Huéspedes";
                        groupBox1.Text = "Mostrar Huésped";
                        groupBox2.Text = "Mostrar Huéspedes";
                        AgregarBotones(btnBuscar, btnRestaurar);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        panel5.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        panel4.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnRegHabit":
                    {
                        this.Text = "Registrar Habitación";
                        groupBox1.Text = "Registrar Nueva Habitación";
                        lblInstruc.Text = "Por favor ingrese los datos de la habitación";
                        AgregarBotones(btnRegistrar, btnLimpiar);
                        groupBox2.Visible = false;
                        this.Size = MinimumSize;
                        label2.Text = "Código Hotel:";
                        label3.Text = "Categoría:";
                        label4.Text = "Soleada:";
                        label5.Text = "Lavado:";
                        label6.Text = "Nevera:";
                        label7.Text = "Precio:";
                        label8.Text = "Cantidad Personas:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        AgregarCombos();
                        break;
                    }
                case "btnModHabit":
                    {
                        this.Text = "Modificar Habitación";
                        groupBox1.Text = "Modificar Habitación";
                        groupBox2.Text = "Mobiliario de la Habitación";
                        lblInstruc.Text = "Por favor ingrese los datos de la habitación";
                        AgregarBotones();
                        label1.Text = "Código Habitación:";
                        label2.Text = "Código Hotel:";
                        label3.Text = "Categoría:";
                        label4.Text = "Soleada:";
                        label5.Text = "Lavado:";
                        label6.Text = "Nevera:";
                        label7.Text = "Precio:";
                        label8.Text = "Cantidad Personas:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        combo1.Enabled = false;
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        AgregarCombos();
                        break;
                    }
                case "btnShowHabit":
                    {
                        this.Text = "Mostrar Habitación";
                        groupBox1.Text = "Mostrar Habitación";
                        groupBox2.Text = "Habitaciones por Hotel";
                        lblInstruc.Text = "Por favor ingrese los datos de la habitación";
                        AgregarBotones(btnBuscar, btnRestaurar);
                        label1.Text = "Código Habitación:";
                        label2.Text = "Código Hotel:";
                        label3.Text = "Categoría:";
                        label4.Text = "Soleada:";
                        label5.Text = "Lavado:";
                        label6.Text = "Nevera:";
                        label7.Text = "Precio:";
                        label8.Text = "Cantidad Personas:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        combo1.Enabled = false;
                        txt7.Enabled = false;
                        combo2.Enabled = false;
                        combo3.Enabled = false;
                        combo4.Enabled = false;
                        combo5.Enabled = false;
                        AgregarCombos();
                        break;
                    }
                case "btnRegMobil":
                    {
                        this.Text = "Registrar Mobiliario";
                        groupBox1.Text = "Registrar Nuevo Mobiliario";
                        lblInstruc.Text = "Por favor ingrese los datos del mobiliario";
                        AgregarBotones(btnRegistrar, btnLimpiar);
                        groupBox2.Visible = false;
                        this.Size = MinimumSize;
                        label2.Text = "Código Habitación:";
                        label3.Text = "Descripción:";
                        label4.Text = "Precio:";
                        label5.Text = "Cantidad:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        break;
                    }
                case "btnModMobil":
                    {
                        this.Text = "Modificar Mobiliario";
                        groupBox1.Text = "Modificar Mobiliario";
                        lblInstruc.Text = "Por favor ingrese los datos del mobiliario";
                        AgregarBotones();
                        groupBox2.Visible = false;
                        this.Size = MinimumSize;
                        label1.Text = "Código Mobiliario:";
                        label2.Text = "Código Habitación:";
                        label3.Text = "Descripción:";
                        label4.Text = "Precio:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnShowMobil":
                    {
                        this.Text = "Mostrar Mobiliario";
                        groupBox1.Text = "Mostrar Mobiliario";
                        groupBox2.Text = "Mostrar Mobiliario por Habitación";
                        lblInstruc.Text = "Por favor ingrese los datos del mobiliario";
                        AgregarBotones(btnBuscar, btnRestaurar);
                        label1.Text = "Código Mobiliario:";
                        label2.Text = "Código Habitación:";
                        label3.Text = "Descripción:";
                        label4.Text = "Precio:";
                        txt4.Enabled = false;
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnRegReser":
                    {
                        panel9.Visible = false;
                        this.Size = MinimumSize;
                        groupBox2.Visible = false;
                        AgregarBotones(btnRegistrar, btnLimpiar);
                        this.Text = "Registrar Reservación";
                        groupBox1.Text = "Registrar Nueva Reservación";
                        lblInstruc.Text = "Por favor ingrese los datos de la reserva";
                        label1.Text = "Cédula Cliente:";
                        label2.Text = "Código Hotel:";
                        label3.Text = "Código Habitación:";
                        label4.Text = "Fecha Llegada:";
                        label5.Text = "Fecha Salida:";
                        label6.Text = "Forma Pago:";
                        label7.Text = "Voucher:";
                        label8.Text = "Monto Tarjeta:";
                        label9.Text = "Monto Efectivo:";
                        panel4.Controls.Remove(txt4);
                        panel5.Controls.Remove(txt5);
                        panel6.Controls.Remove(txt6);
                        panel6.Controls.Add(combo6);
                        panel4.Controls.Add(dateTimePicker1);
                        panel5.Controls.Add(dateTimePicker2);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        flptxt.Controls.Add(panel9);
                        break;
                    }
                case "btnModReser":
                    {
                        AgregarBotones();
                        panel9.Visible = false;
                        this.Text = "Modificar Reservación";
                        groupBox1.Text = "Modificar Reservación";
                        groupBox2.Text = "Historial de Reservaciones del Cliente";
                        lblInstruc.Text = "Por favor ingrese los datos de la reserva";
                        label1.Text = "Cédula Cliente:";
                        label2.Text = "Código Hotel:";
                        label3.Text = "Código Habitación:";
                        label4.Text = "Fecha Llegada:";
                        label5.Text = "Fecha Salida:";
                        label6.Text = "Forma Pago:";
                        label7.Text = "Voucher:";
                        label8.Text = "Monto Tarjeta:";
                        label9.Text = "Monto Efectivo:";
                        label10.Text = "Código Reserva:";
                        panel4.Controls.Remove(txt4);
                        panel5.Controls.Remove(txt5);
                        panel6.Controls.Remove(txt6);
                        panel6.Controls.Add(combo6);
                        panel4.Controls.Add(dateTimePicker1);
                        panel5.Controls.Add(dateTimePicker2);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel11);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        flptxt.Controls.Add(panel9);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        panel11.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnShowReser":
                    {
                        AgregarBotones(btnBuscar, btnRestaurar);
                        panel9.Visible = false;
                        this.Text = "Mostrar Reservación";
                        groupBox1.Text = "Mostrar Reservación";
                        groupBox2.Text = "Mostrar Reservaciones Activas";
                        lblInstruc.Text = "Por favor ingrese los datos de la reserva";
                        label1.Text = "Cédula Cliente:";
                        label2.Text = "Código Hotel:";
                        label3.Text = "Código Habitación:";
                        label4.Text = "Fecha Llegada:";
                        label5.Text = "Fecha Salida:";
                        label6.Text = "Forma Pago:";
                        label7.Text = "Voucher:";
                        label8.Text = "Monto Tarjeta:";
                        label9.Text = "Monto Efectivo:";
                        label10.Text = "Código Reserva:";
                        combo6.Enabled = false;
                        txt7.Enabled = false;
                        txt8.Enabled = false;
                        txt9.Enabled = false;
                        panel4.Controls.Remove(txt4);
                        panel5.Controls.Remove(txt5);
                        panel6.Controls.Remove(txt6);
                        panel6.Controls.Add(combo6);
                        panel4.Controls.Add(dateTimePicker1);
                        panel5.Controls.Add(dateTimePicker2);
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel11);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        flptxt.Controls.Add(panel8);
                        flptxt.Controls.Add(panel9);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        panel2.BackColor = Color.FromArgb(167, 47, 45);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        panel11.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnRegHotel":
                    {
                        this.Text = "Registrar Hotel";
                        groupBox1.Text = "Registrar Nuevo Hotel";
                        lblInstruc.Text = "Por favor ingrese los datos del Hotel";
                        AgregarBotones(btnRegistrar, btnLimpiar);
                        groupBox2.Visible = false;
                        this.Size = MinimumSize;
                        label2.Text = "País:";
                        label3.Text = "Provincia:";
                        label4.Text = "Canton:";
                        label5.Text = "Dirección exacta:";
                        label6.Text = "Teléfono 1:";
                        label7.Text = "Teléfono 2:";
                        label8.Text = "Teléfono 3:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel10);
                        panel6.Controls.Add(btnCancel3);
                        panel6.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        btnCancel3.Visible = true;
                        btnCancel2.Visible = true;
                        btnCancel1.Visible = true;
                        break;
                    }
                case "btnModHotel":
                    {
                        this.Text = "Modificar Hotel";
                        groupBox1.Text = "Modificar Hotel";
                        groupBox2.Text = "Habitaciones del Hotel";
                        lblInstruc.Text = "Por favor ingrese los datos del Hotel";
                        AgregarBotones();
                        label1.Text = "Código Hotel:";
                        label2.Text = "País:";
                        label3.Text = "Provincia:";
                        label4.Text = "Canton:";
                        label5.Text = "Dirección exacta:";
                        label6.Text = "Teléfono 1:";
                        label7.Text = "Teléfono 2:";
                        label8.Text = "Teléfono 3:";
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel10);
                        panel6.Controls.Add(btnCancel3);
                        panel6.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        btnCancel3.Visible = true;
                        btnCancel2.Visible = true;
                        btnCancel1.Visible = true;
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }
                case "btnShowHotel":
                    {
                        this.Text = "Mostrar Hotel";
                        groupBox1.Text = "Mostrar Hotel";
                        groupBox2.Text = "Mostrar Hoteles";
                        lblInstruc.Text = "Por favor ingrese los datos del Hotel";
                        AgregarBotones(btnBuscar, btnRestaurar);
                        label1.Text = "Código Hotel:";
                        label2.Text = "País:";
                        label3.Text = "Provincia:";
                        label4.Text = "Canton:";
                        label5.Text = "Dirección exacta:";
                        label6.Text = "Teléfono 1:";
                        label7.Text = "Teléfono 2:";
                        label8.Text = "Teléfono 3:";
                        txt2.Enabled = false;
                        txt3.Enabled = false;
                        txt4.Enabled = false;
                        txt5.Enabled = false;
                        txt6.Enabled = false;
                        txt7.Enabled = false;
                        txt8.Enabled = false;
                        comboTel1.Enabled = false;
                        comboTel2.Enabled = false;
                        comboTel3.Enabled = false;
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel1);
                        flptxt.Controls.Add(panel2);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        panel6.Controls.Add(btnCancel3);
                        panel6.Controls.Add(comboTel1);
                        panel7.Controls.Add(comboTel2);
                        panel8.Controls.Add(comboTel3);
                        panel1.BackColor = Color.FromArgb(167, 47, 45);
                        HotelBLL hotelbll = new HotelBLL();
                        // obtiene los datos de la list<> habitación encontrada para mostrarlos en el grid view
                        dataGridView1.DataSource = hotelbll.ListarTodosLosHoteles();
                        // Cambia el nombre de las column headers
                        dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                        dataGridView1.Columns["DPais"].HeaderText = "País";
                        dataGridView1.Columns["DProvincia"].HeaderText = "Provincia";
                        dataGridView1.Columns["DCanton"].HeaderText = "Cantón";
                        dataGridView1.Columns["DExacta"].HeaderText = "Dirección Exacta";
                        dataGridView1.Columns["Activo"].Visible = false;
                        break;
                    }
                case "btnConta":
                    {
                        AgregarBotones(btnBuscar, btnLimpiar);
                        this.Text = "Contabilidad";
                        groupBox1.Text = "Contabilidad";
                        lblInstruc.Text = "Por favor ingrese los datos de la transacción";
                        groupBox2.Text = "Desglose";
                        label3.Text = "Código Hotel:";
                        label4.Text = "Fecha Inicial:";
                        label5.Text = "Fecha Final:";
                        label6.Text = "Total Efectivo:";
                        label7.Text = "Total Tarjeta:";
                        txt6.Enabled = false;
                        txt7.Enabled = false;
                        flptxt.Controls.Add(pnlInstruc);
                        flptxt.Controls.Add(panel3);
                        flptxt.Controls.Add(panel4);
                        flptxt.Controls.Add(panel5);
                        flptxt.Controls.Add(panel6);
                        flptxt.Controls.Add(panel7);
                        panel4.Controls.Remove(txt4);
                        panel5.Controls.Remove(txt5);
                        panel4.Controls.Add(dateTimePicker1);
                        panel5.Controls.Add(dateTimePicker2);
                        panel3.BackColor = Color.FromArgb(167, 47, 45);
                        panel4.BackColor = Color.FromArgb(167, 47, 45);
                        panel5.BackColor = Color.FromArgb(167, 47, 45);
                        break;
                    }

            }
        }

        // Botones de accion en CRUD

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnRegHuesped":
                    {
                        // Crear una nueva instancia de Cliente
                        Cliente nuevoCliente = new Cliente();
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            // Asignar los valores de los campos de texto al nuevo cliente
                            nuevoCliente.IDCliente = Convert.ToInt32(txt1.Text);
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar una cédula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text))
                        {
                            nuevoCliente.Nombre = txt2.Text;
                            nuevoCliente.Apellido1 = txt3.Text;
                            nuevoCliente.Apellido2 = txt4.Text;
                        }
                        // Comprueba si el usuario ingreso telefonos en todos los campos de telefonos
                        // Si lo hizo, este manda los telefonos a la lista telefonos del Usuario
                        if (txt5.Text != "")
                        {
                            if (comboTel1.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono1 = new TelefonoCliente();
                                //telefono1.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono1.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel1.SelectedIndex]);
                                telefono1.Numero = txt5.Text;
                                nuevoCliente.Telefonos.Add(telefono1);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar por lo menos un Teléfono",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (txt7.Text != "")
                        {
                            if (comboTel2.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono2 = new TelefonoCliente();
                                //telefono2.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono2.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel2.SelectedIndex]);
                                telefono2.Numero = txt7.Text;
                                nuevoCliente.Telefonos.Add(telefono2);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        if (txt8.Text != "")
                        {
                            if (comboTel3.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono3 = new TelefonoCliente();
                                //telefono3.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono3.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel3.SelectedIndex]);
                                telefono3.Numero = txt8.Text;
                                nuevoCliente.Telefonos.Add(telefono3);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        DialogResult respuesta = MessageBox.Show($"Desea realizar el registro de {nuevoCliente.Nombre}",
                            "Reservar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);  

                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de ClienteBLL
                            ClienteBLL clienteBLL = new ClienteBLL();

                            // Intentar insertar el nuevo cliente
                            ResultadoOperacion resultado;

                            resultado = clienteBLL.InsertarCliente(nuevoCliente);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        

                    }
                    break;
                case "btnRegHabit":
                    {
                        // Crear una nueva instancia de Habitacion
                        Habitacion nuevaHabitacion = new Habitacion();
                        if (!string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt7.Text))
                        {
                            // Asignar los valores de los campos de texto a la nueva habitación
                            nuevaHabitacion.CodHotel = Convert.ToInt32(txt2.Text);
                            nuevaHabitacion.Precio = Convert.ToDecimal(txt7.Text);
                        }
                        else
                        {
                            MessageBox.Show("Debe rellenar todos los campos.",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (combo1.SelectedIndex != -1 &&
                            combo2.SelectedIndex != -1 &&
                            combo3.SelectedIndex != -1 &&
                            combo4.SelectedIndex != -1 &&
                            combo5.SelectedIndex != -1)
                        {
                            nuevaHabitacion.Categ = Categoria[combo1.SelectedIndex];
                            if (combo2.SelectedIndex == 0)
                            {
                                nuevaHabitacion.Soleada = true;
                            }
                            else
                            {
                                nuevaHabitacion.Soleada = false;
                            }

                            if (combo3.SelectedIndex == 0)
                            {
                                nuevaHabitacion.Lavado = true;
                            }
                            else
                            {
                                nuevaHabitacion.Lavado = false;
                            }

                            if (combo4.SelectedIndex == 0)
                            {
                                nuevaHabitacion.Nevera = true;
                            }
                            else
                            {
                                nuevaHabitacion.Nevera = false;
                            }
                            nuevaHabitacion.CantPers = Convert.ToInt32(numeros[combo5.SelectedIndex]);
                        }
                        else
                        {
                            MessageBox.Show("Debe rellenar todos los combos.",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        nuevaHabitacion.Estado = "Libre";

                        // Crear una nueva instancia de HabitacionBLL
                        HabitacionBLL habitacionBLL = new HabitacionBLL();

                        // Intentar insertar la nueva habitación
                        ResultadoOperacion resultado = habitacionBLL.InsertarHabitacion(nuevaHabitacion);

                        if (resultado.Exito)
                        {
                            MessageBox.Show(resultado.Mensaje,
                                    "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(resultado.Mensaje,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnRegMobil":
                    {
                        // Crear una nueva instancia de Mobiliario
                        Mobiliario nuevoMobiliario = new Mobiliario();
                        // Asignar los valores de los campos de texto al nuevo mobiliario
                        int cantidad;
                        if (!string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text) &&
                            !string.IsNullOrEmpty(txt5.Text))
                        {
                            nuevoMobiliario.CodMobil = txt1.Text;
                            nuevoMobiliario.CodHabit = txt2.Text;
                            nuevoMobiliario.Descripcion = txt3.Text;
                            nuevoMobiliario.Precio = Convert.ToDecimal(txt4.Text);
                            cantidad = Convert.ToInt32(txt5.Text);
                        }
                        else
                        {
                            MessageBox.Show("Debe rellenar todos los campos.",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Crear una nueva instancia de MobiliarioBLL
                        MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();

                        // Insertar el nuevo mobiliario
                        ResultadoOperacion resultado = mobiliarioBLL.Insertar(nuevoMobiliario, cantidad);

                        if (resultado.Exito)
                        {
                            MessageBox.Show(resultado.Mensaje,
                                    "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(resultado.Mensaje,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    break;
                case "btnRegReser":
                    {
                        // Crear una nueva instancia de Reservacion
                        Reservacion nuevaReservacion = new Reservacion();
                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            combo6.SelectedIndex != -1)
                        {
                            nuevaReservacion.IdCliente = Convert.ToInt32(txt1.Text);
                            nuevaReservacion.CodHotel = Convert.ToInt32(txt2.Text);
                            nuevaReservacion.CodHabit = txt3.Text;
                            nuevaReservacion.FechaLlegada = dateTimePicker1.Value;
                            nuevaReservacion.FechaSalida = dateTimePicker2.Value;
                            nuevaReservacion.FormaPago = fpago[combo6.SelectedIndex];
                            nuevaReservacion.Voucher = txt7.Text;
                        }
                        else
                        {
                            MessageBox.Show("Debe completar todos los campos",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Asignar los valores de los campos de texto a la nueva reservación
                        switch (combo6.SelectedIndex)
                        {
                            case 0: // Efectivo
                                if (!string.IsNullOrEmpty(txt9.Text))
                                {
                                    nuevaReservacion.MontoTarjeta = 0;
                                    nuevaReservacion.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en efectivo",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                            case 1: // Tarjeta
                                if (!string.IsNullOrEmpty(txt8.Text))
                                {
                                    nuevaReservacion.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                                    nuevaReservacion.MontoEfectivo = 0;
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en tarjeta",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                if (txt7.Text == "" || txt7.Text == null)
                                {
                                    MessageBox.Show("Debe ingresar el Voucher",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                            case 2: // Mixto
                                if (!string.IsNullOrEmpty(txt8.Text) &&
                                    !string.IsNullOrEmpty(txt9.Text))
                                {
                                    nuevaReservacion.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                                    nuevaReservacion.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en tarjeta y en efectivo",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                if (txt7.Text == "" || txt7.Text == null)
                                {
                                    MessageBox.Show("Debe ingresar el Voucher",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                        }

                        // Crear una nueva instancia de ReservacionBLL
                        ReservacionBLL reservacionBLL = new ReservacionBLL();

                        // Crear la nueva reserva
                        ResultadoOperacion resultado = reservacionBLL.CrearReserva(nuevaReservacion);

                        if (resultado.Exito)
                        {
                            MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    break;
                case "btnRegHotel":
                    {
                        // Crear una nueva instancia de Hotel
                        Hotel nuevoHotel = new Hotel();

                        // Asignar los valores de los campos de texto al nuevo hotel
                        if (!string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text) &&
                            !string.IsNullOrEmpty(txt5.Text))
                        {
                            nuevoHotel.DPais = txt2.Text;
                            nuevoHotel.DProvincia = txt3.Text;
                            nuevoHotel.DCanton = txt4.Text;
                            nuevoHotel.DExacta = txt5.Text;
                        }
                        else
                        {
                            MessageBox.Show("Debe completar todos los campos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Agregar los nuevos teléfonos a la lista de teléfonos del hotel
                        if (txt6.Text != "" && txt6.Text != null)
                        {
                            if (comboTel1.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono1 = new TelefonoHotel();
                                telefono1.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel1.SelectedIndex]);
                                telefono1.Numero = txt6.Text;
                                nuevoHotel.Telefonos.Add(telefono1);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar por lo menos un Teléfono",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        if (txt7.Text != "")
                        {
                            if (comboTel2.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono2 = new TelefonoHotel();
                                telefono2.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel2.SelectedIndex]);
                                telefono2.Numero = txt7.Text;
                                nuevoHotel.Telefonos.Add(telefono2);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        if (txt8.Text != "")
                        {
                            if (comboTel3.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono3 = new TelefonoHotel();
                                telefono3.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel3.SelectedIndex]);
                                telefono3.Numero = txt8.Text;
                                nuevoHotel.Telefonos.Add(telefono3);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Crear una nueva instancia de HotelBLL
                        HotelBLL hotelBLL = new HotelBLL();

                        ResultadoOperacion resultado = hotelBLL.InsertarHotel(nuevoHotel);

                        if (resultado.Exito)
                        {
                            MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnModRegis":
                    {
                        // Crear una nueva instancia de Cliente
                        Cliente cliente = new Cliente();

                        // Asignar los valores de los campos de texto al cliente
                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text))
                        {
                            cliente.IDCliente = Convert.ToInt32(txt1.Text);
                            cliente.Nombre = txt2.Text;
                            cliente.Apellido1 = txt3.Text;
                            cliente.Apellido2 = txt4.Text;
                        }
                        else
                        {
                            MessageBox.Show("Todos los campos deben tener datos",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (cliente.IDCliente != clienteGeneral.IDCliente)
                        {
                            MessageBox.Show("No se puede cambiar la cédula del Cliente.");
                            return;
                        }

                        // Limpiar la lista de teléfonos del cliente y agregar los nuevos teléfonos
                        cliente.Telefonos.Clear();

                        if (txt5.Text != "")
                        {
                            if (comboTel1.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono1 = new TelefonoCliente();
                                telefono1.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono1.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel1.SelectedIndex]);
                                telefono1.Numero = txt5.Text;
                                cliente.Telefonos.Add(telefono1);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El cliente debe de tener por lo menos un Teléfono",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (txt7.Text != "")
                        {
                            if (comboTel2.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono2 = new TelefonoCliente();
                                telefono2.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono2.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel2.SelectedIndex]);
                                telefono2.Numero = txt7.Text;
                                cliente.Telefonos.Add(telefono2);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        if (txt8.Text != "")
                        {
                            if (comboTel3.SelectedIndex != -1)
                            {
                                TelefonoCliente telefono3 = new TelefonoCliente();
                                telefono3.IDCliente = Convert.ToInt32(txt1.Text);
                                telefono3.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel3.SelectedIndex]);
                                telefono3.Numero = txt8.Text;
                                cliente.Telefonos.Add(telefono3);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea Actualizar este registro?",
                            "Actualizar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de ClienteDAL
                            ClienteBLL clienteBLL = new ClienteBLL();

                            // Intentar actualizar el cliente
                            ResultadoOperacion resultado = clienteBLL.ModificarCliente(cliente);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    break;
                case "btnModHabit":
                    {
                        // Crear una nueva instancia de Habitacion
                        Habitacion habitacion = new Habitacion();

                        // Asignar los valores de los campos de texto a la habitación existente
                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt7.Text) &&
                            combo1.SelectedIndex != -1 &&
                            combo2.SelectedIndex != -1 &&
                            combo3.SelectedIndex != -1 &&
                            combo4.SelectedIndex != -1 &&
                            combo5.SelectedIndex != -1)
                        {

                            habitacion.CodHotel = Convert.ToInt32(txt2.Text);

                            habitacion.Categ = Categoria[combo1.SelectedIndex]; // Deberiamos de conservar esto aquí si no se puede modificar?

                            if (combo2.SelectedIndex == 0)
                            {
                                habitacion.Soleada = true;
                            }
                            else
                            {
                                habitacion.Soleada = false;
                            }

                            if (combo3.SelectedIndex == 0)
                            {
                                habitacion.Lavado = true;
                            }
                            else
                            {
                                habitacion.Lavado = false;
                            }

                            if (combo4.SelectedIndex == 0)
                            {
                                habitacion.Nevera = true;
                            }
                            else
                            {
                                habitacion.Nevera = false;
                            }
                            habitacion.Precio = Convert.ToDecimal(txt7.Text);

                            habitacion.CantPers = Convert.ToInt32(numeros[combo5.SelectedIndex]);

                            habitacion.Estado = ""; //El estado no se puede modificar, solo al reservar y al eliminar

                            habitacion.CodHabit = txt1.Text;
                        }
                        else
                        {
                            MessageBox.Show("Debe Rellenar todos los campos",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (habitacion.CodHabit != habitacionGeneral.CodHabit)
                        {
                            MessageBox.Show("No se puede cambiar la código de la habitación.");
                            return;
                        }
                        if (habitacion.CodHotel != habitacionGeneral.CodHotel)
                        {
                            MessageBox.Show($"No se puede cambiar el Código de Hotel: {habitacionGeneral.CodHotel}");
                            return;
                        }                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea Actualizar esta Habitación?",
                            "Actualizar Habitación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de HabitacionBLL
                            HabitacionBLL habitacionBLL = new HabitacionBLL();

                            // Intentar actualizar la habitación
                            ResultadoOperacion resultado = habitacionBLL.ActualizarHabitacion(habitacion);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModMobil":
                    {

                        // Crear una nueva instancia de Mobiliario
                        Mobiliario mobiliario = new Mobiliario();

                        // Asignar los valores de los campos de texto al mobiliario existente
                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text))
                        {
                            mobiliario.CodMobil = txt1.Text;
                            mobiliario.CodHabit = txt2.Text;
                            mobiliario.Descripcion = txt3.Text;
                            mobiliario.Precio = Convert.ToDecimal(txt4.Text);
                        }
                        else
                        {
                            MessageBox.Show("Debe Rellenar todos los campos",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (mobiliario.CodMobil != mobiliarioGeneral.CodMobil)
                        {
                            MessageBox.Show($"No se puede cambiar el Código de Mobiliario: {mobiliarioGeneral.CodMobil}");
                            return;
                        }

                        if (mobiliario.CodHabit != mobiliarioGeneral.CodHabit)
                        {
                            MessageBox.Show($"No se puede cambiar la Código de Habitación: {mobiliarioGeneral.CodHabit}");
                            return;
                        }


                        string DescripcionBase1 = mobiliario.Descripcion.Split(' ')[0];
                        string DescripcionBase2 = mobiliarioGeneral.Descripcion.Split(' ')[0];

                        if (DescripcionBase1 != DescripcionBase2)
                        {
                            MessageBox.Show($"No se puede cambiar la palabra base de la descripción del mobiliario: {DescripcionBase2}");
                            return;
                        }                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea Actualizar este registro?",
                            "Reservar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de MobiliarioBLL
                            MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();

                            // Verificar si el mobiliario existe antes de intentar actualizarlo
                            Mobiliario existente = mobiliarioBLL.SeleccionarPorId(mobiliario.CodMobil);
                            if (existente == null)
                            {
                                MessageBox.Show($"No existe un mobiliario con el código {mobiliario.CodMobil}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Intentar actualizar el mobiliario
                            ResultadoOperacion resultado = mobiliarioBLL.Actualizar(mobiliario);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModReser":
                    {
                        // Crear una nueva instancia de Reservacion
                        Reservacion UpdateReserva = new Reservacion();
                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt10.Text) &&
                            combo6.SelectedIndex != -1)
                        {
                            // Asignar los valores de los campos de texto a la nueva reservación
                            UpdateReserva.CodReserva = txt10.Text;
                            UpdateReserva.IdCliente = Convert.ToInt32(txt1.Text);
                            UpdateReserva.CodHotel = Convert.ToInt32(txt2.Text);
                            UpdateReserva.CodHabit = txt3.Text;
                            UpdateReserva.FechaLlegada = dateTimePicker1.Value;
                            UpdateReserva.FechaSalida = dateTimePicker2.Value;
                            UpdateReserva.FormaPago = fpago[combo6.SelectedIndex];
                            UpdateReserva.Voucher = txt7.Text;
                        }
                        else
                        {
                            MessageBox.Show("Debe Rellenar todos los campos",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        switch (combo6.SelectedIndex)
                        {
                            case 0: // Efectivo
                                if (!string.IsNullOrEmpty(txt9.Text))
                                {
                                    UpdateReserva.MontoTarjeta = 0;
                                    UpdateReserva.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en efectivo",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                            case 1: // Tarjeta
                                if (!string.IsNullOrEmpty(txt8.Text))
                                {
                                    UpdateReserva.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                                    UpdateReserva.MontoEfectivo = 0;
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en tarjeta",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                if (txt7.Text == "" || txt7.Text == null)
                                {
                                    MessageBox.Show("Debe ingresar el Voucher",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                            case 2: // Mixto
                                if (!string.IsNullOrEmpty(txt8.Text) &&
                                    !string.IsNullOrEmpty(txt9.Text))
                                {
                                    UpdateReserva.MontoTarjeta = Convert.ToDecimal(txt8.Text);
                                    UpdateReserva.MontoEfectivo = Convert.ToDecimal(txt9.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Debe ingresar un monto en tarjeta y en efectivo",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                if (txt7.Text == "" || txt7.Text == null)
                                {
                                    MessageBox.Show("Debe ingresar el Voucher",
                                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                break;
                        }

                        if (UpdateReserva.CodReserva != ReserGeneral.CodReserva)
                        {
                            MessageBox.Show($"No se puede cambiar el código de reservació\nCódigo Reservación: {ReserGeneral.CodReserva}");
                            return;
                        }

                        if (UpdateReserva.CodHotel != ReserGeneral.CodHotel)
                        {
                            MessageBox.Show($"No se puede cambiar el código de hotel\nCódigo Hotel: {ReserGeneral.CodHotel}");
                            return;
                        }                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea Actualizar este registro?",
                            "Reservar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de ReservacionBLL
                            ReservacionBLL reservacionBLL = new ReservacionBLL();

                            ResultadoOperacion resultado = reservacionBLL.ModificarReserva(UpdateReserva, UpdateReserva.IdCliente, ReserGeneral.CodHabit);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModHotel":
                    {
                        // Crear una nueva instancia de Hotel
                        Hotel hotelModificado = new Hotel();

                        if (!string.IsNullOrEmpty(txt1.Text) &&
                            !string.IsNullOrEmpty(txt2.Text) &&
                            !string.IsNullOrEmpty(txt3.Text) &&
                            !string.IsNullOrEmpty(txt4.Text) &&
                            !string.IsNullOrEmpty(txt5.Text))
                        {

                            // Asignar los valores de los campos de texto al hotel modificado
                            hotelModificado.CodHotel = Convert.ToInt32(txt1.Text); // codHotel es consecutivo o sea no se puede asignar
                            hotelModificado.DPais = txt2.Text;
                            hotelModificado.DProvincia = txt3.Text;
                            hotelModificado.DCanton = txt4.Text;
                            hotelModificado.DExacta = txt5.Text;
                        }
                        // Agregar los nuevos teléfonos a la lista de teléfonos del hotel
                        if (txt6.Text != "" && txt6.Text != null)
                        {
                            if (comboTel1.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono1 = new TelefonoHotel();
                                telefono1.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel1.SelectedIndex]);
                                telefono1.Numero = txt6.Text;
                                hotelModificado.Telefonos.Add(telefono1);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        }
                        else
                        {
                            MessageBox.Show("El cliente debe tener por lo menos un Teléfono",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (txt7.Text != "")
                        {
                            if (comboTel2.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono2 = new TelefonoHotel();
                                telefono2.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel2.SelectedIndex]);
                                telefono2.Numero = txt7.Text;
                                hotelModificado.Telefonos.Add(telefono2);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        if (txt8.Text != "")
                        {
                            if (comboTel3.SelectedIndex != -1)
                            {
                                TelefonoHotel telefono3 = new TelefonoHotel();
                                telefono3.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel3.SelectedIndex]);
                                telefono3.Numero = txt8.Text;
                                hotelModificado.Telefonos.Add(telefono3);
                            }
                            else
                            {
                                MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        if (hotelModificado.CodHotel != hotelGeneral.CodHotel)
                        {
                            MessageBox.Show($"No se puede cambiar la Código de Hotel: {hotelGeneral.CodHotel}");
                            return;
                        }                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea Actualizar este registro?",
                            "Reservar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de HotelBLL
                            HotelBLL hotelBLL = new HotelBLL();

                            // Llama al método de actualización en lugar del método de inserción
                            ResultadoOperacion resultado = hotelBLL.ActualizarHotel(hotelModificado); // Si no hubo excepciones, la actualización fue exitosa

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnModRegis":
                    {
                        // Asegúrate de que el campo del ID del cliente esté lleno
                        if (string.IsNullOrWhiteSpace(txt1.Text))
                        {
                            MessageBox.Show("El campo del ID del cliente debe estar lleno");
                            return;
                        }

                        // Obtener el ID del cliente del campo de texto
                        int idCliente = Convert.ToInt32(txt1.Text);                       

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar este registro?",
                            "Eliminar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de ClienteBLL
                            ClienteBLL clienteBLL = new ClienteBLL();

                            // Intentar eliminar el cliente y obtener el resultado
                            ResultadoOperacion resultado = clienteBLL.EliminarCliente(idCliente);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModHabit":
                    {
                        // Asegúrate de que el campo del código de la habitación esté lleno
                        if (string.IsNullOrWhiteSpace(txt1.Text))
                        {
                            MessageBox.Show("El campo del código de la habitación debe estar lleno");
                            return;
                        }

                        // Obtener el código de la habitación del campo de texto
                        string codHabitacion = txt1.Text;

                        // Crear una nueva instancia de HabitacionBLL
                        HabitacionBLL habitacionBLL = new HabitacionBLL();

                        // Intentar eliminar la habitación si está libre y obtener el resultado
                        bool resultado;                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar esta habitación?",
                            "Eliminar Habitación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            try
                            {
                                resultado = habitacionBLL.EliminarSiEstaLibre(codHabitacion);

                                if (resultado)
                                {
                                    MessageBox.Show("Habitación eliminada con éxito",
                                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Hubo un error al eliminar la habitación o la habitación está reservada",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hubo un error al eliminar la habitación: {ex.Message}");
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModMobil":
                    {
                        // Asegúrate de que el campo del código del mobiliario esté lleno
                        if (string.IsNullOrWhiteSpace(txt1.Text))
                        {
                            MessageBox.Show("El campo del código del mobiliario debe estar lleno");
                            return;
                        }

                        // Obtener el código del mobiliario del campo de texto
                        string codMobil = txt1.Text;                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar este mobiliario?",
                            "Eliminar Mobiliario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de MobiliarioBLL
                            MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();

                            // Intentar eliminar el mobiliario
                            ResultadoOperacion resultado = mobiliarioBLL.Eliminar(codMobil);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModReser":
                    {
                        // Asegúrate de que el campo del código de la reserva esté lleno
                        if (string.IsNullOrWhiteSpace(txt1.Text))
                        {
                            MessageBox.Show("El campo del código de la reserva debe estar lleno");
                            return;
                        }

                        // Obtener el código de la reserva del campo de texto
                        string codReserva = txt1.Text;
                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar esta reserva?",
                            "Eliminar Reserva", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de ReservacionBLL
                            ReservacionBLL reservacionBLL = new ReservacionBLL();

                            // Intentar eliminar la reserva
                            ResultadoOperacion resultado = reservacionBLL.EliminarReserva(codReserva);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "btnModHotel":
                    {
                        // Asegúrate de que el campo del código del hotel esté lleno
                        if (string.IsNullOrWhiteSpace(txt1.Text))
                        {
                            MessageBox.Show("El campo del código del hotel debe estar lleno");
                            return;
                        }

                        // Obtener el código del hotel del campo de texto
                        int codHotel = Convert.ToInt32(txt1.Text);
                        

                        DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar este Hotel?",
                            "Eliminar Hotel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (respuesta == DialogResult.Yes)
                        {
                            // Crear una nueva instancia de HotelBLL
                            HotelBLL hotelBLL = new HotelBLL();

                            // Intentar eliminar el hotel
                            ResultadoOperacion resultado = hotelBLL.EliminarHotel(codHotel);

                            if (resultado.Exito)
                            {
                                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (respuesta == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo una respuesta inesperada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            Cliente nuevoCliente = new Cliente();

            if (!string.IsNullOrEmpty(txt1.Text))
            {
                // Asignar los valores de los campos de texto al nuevo cliente
                nuevoCliente.IDCliente = Convert.ToInt32(txt1.Text);
            }
            else
            {
                MessageBox.Show("Debe ingresar una cédula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(txt2.Text) &&
                !string.IsNullOrEmpty(txt3.Text) &&
                !string.IsNullOrEmpty(txt4.Text))
            {
                nuevoCliente.Nombre = txt2.Text;
                nuevoCliente.Apellido1 = txt3.Text;
                nuevoCliente.Apellido2 = txt4.Text;
            }
            // Comprueba si el usuario ingreso telefonos en todos los campos de telefonos
            // Si lo hizo, este manda los telefonos a la lista telefonos del Usuario
            if (txt5.Text != "")
            {
                if (comboTel1.SelectedIndex != -1)
                {
                    TelefonoCliente telefono1 = new TelefonoCliente();
                    //telefono1.IDCliente = Convert.ToInt32(txt1.Text);
                    telefono1.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel1.SelectedIndex]);
                    telefono1.Numero = txt5.Text;
                    nuevoCliente.Telefonos.Add(telefono1);
                }
                else
                {
                    MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar por lo menos un Teléfono",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (txt7.Text != "")
            {
                if (comboTel2.SelectedIndex != -1)
                {
                    TelefonoCliente telefono2 = new TelefonoCliente();
                    //telefono2.IDCliente = Convert.ToInt32(txt1.Text);
                    telefono2.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel2.SelectedIndex]);
                    telefono2.Numero = txt7.Text;
                    nuevoCliente.Telefonos.Add(telefono2);
                }
                else
                {
                    MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (txt8.Text != "")
            {
                if (comboTel3.SelectedIndex != -1)
                {
                    TelefonoCliente telefono3 = new TelefonoCliente();
                    //telefono3.IDCliente = Convert.ToInt32(txt1.Text);
                    telefono3.CodigoPais = Convert.ToInt32(codigos_telefono[comboTel3.SelectedIndex]);
                    telefono3.Numero = txt8.Text;
                    nuevoCliente.Telefonos.Add(telefono3);
                }
                else
                {
                    MessageBox.Show("Debe indicar el código de país del Teléfono ingresado",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            // Crear una nueva instancia de ClienteBLL
            ClienteBLL clienteBLL = new ClienteBLL();

            // Intentar insertar el nuevo cliente
            ResultadoOperacion resultado = clienteBLL.InsertarCliente(nuevoCliente);

            if (resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (txt1.Text != "")
                {
                    ReservacionesForm ventanaReser = new ReservacionesForm();
                    ventanaReser.BotonPresionado = "btnRegisReser";
                    ventanaReser.Vartxt1 = txt1.Text;
                    ventanaReser.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No se ingreso una cedula", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnModRegis":
                    {
                        // Verifica si se ingresó un ID de cliente
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int idCliente;
                            if (int.TryParse(txt1.Text, out idCliente))
                            {
                                try
                                {
                                    ClienteBLL clienteBLL = new ClienteBLL();
                                    // Busca el cliente por ID
                                    Cliente cliente = clienteBLL.seleccionarPorId(idCliente);
                                    // Guardamos en clienteGeneral los valores obtenidos de la busqueda para validar los
                                    // datos modificados por el usuario
                                    clienteGeneral = new Cliente();
                                    clienteGeneral = cliente;
                                    if (cliente != null)
                                    {
                                        // Rellena los TextBox con la información del cliente encontrado
                                        txt2.Text = cliente.Nombre;
                                        txt3.Text = cliente.Apellido1;
                                        txt4.Text = cliente.Apellido2;
                                        // Asigna los números de teléfono a los TextBox correspondientes
                                        if (cliente.Telefonos.Count >= 1)
                                        {
                                            PanelIndex = 0;
                                            btnCancel1.Visible = true;
                                            txt5.Text = cliente.Telefonos[0].Numero;
                                            comboTel1.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt7.Text = "";
                                            txt8.Text = "";
                                            comboTel2.SelectedIndex = -1;
                                            comboTel3.SelectedIndex = -1;
                                        }
                                        if (cliente.Telefonos.Count >= 2)
                                        {
                                            PanelIndex = 0;
                                            txt7.Text = cliente.Telefonos[1].Numero;
                                            comboTel2.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel10);
                                            flptxt.Controls.Add(panel7);
                                            flptxt.Controls.Add(panel10);
                                            flptxt.Controls.Remove(panel8);
                                            txt8.Text = "";
                                            comboTel3.SelectedIndex = -1;
                                            btnCancel3.Visible = false;
                                            PanelIndex++;
                                        }
                                        if (cliente.Telefonos.Count >= 3)
                                        {
                                            txt8.Text = cliente.Telefonos[2].Numero;
                                            comboTel3.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel10);
                                            flptxt.Controls.Add(panel8);
                                            flptxt.Controls.Add(panel10);
                                            btnCancel1.Visible = false;
                                            btnCancel3.Visible = false;
                                            PanelIndex++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontró a ningún cliente con ese ID",
                                            "NO ENCONTRADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (ArgumentException ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El ID del cliente debe ser un número válido.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        // Verifica si se ingresó un nombre de cliente
                        else if (!string.IsNullOrEmpty(txt2.Text) && !string.IsNullOrEmpty(txt3.Text) && !string.IsNullOrEmpty(txt4.Text))
                        {
                            try
                            {
                                ClienteBLL clienteBLL = new ClienteBLL();
                                // Busca clientes por nombre
                                var clientes = clienteBLL.seleccionarPorNombre(txt2.Text, txt3.Text, txt4.Text);
                                // Si hay al menos un cliente encontrado, muestra los detalles del primer cliente en la lista
                                if (clientes.Count > 0)
                                {
                                    var primerCliente = clientes[0];
                                    clienteGeneral = new Cliente();
                                    clienteGeneral = primerCliente;
                                    txt1.Text = primerCliente.IDCliente.ToString();
                                    // Asigna los números de teléfono a los TextBox correspondientes
                                    txt2.Text = primerCliente.Nombre;
                                    txt3.Text = primerCliente.Apellido1;
                                    txt4.Text = primerCliente.Apellido2;
                                    if (primerCliente.Telefonos.Count >= 1)
                                    {
                                        PanelIndex = 0;
                                        btnCancel1.Visible = true;
                                        txt5.Text = primerCliente.Telefonos[0].Numero;
                                        comboTel1.SelectedIndex = 7;
                                        flptxt.Controls.Remove(panel7);
                                        flptxt.Controls.Remove(panel8);
                                        txt7.Text = "";
                                        txt8.Text = "";
                                        comboTel2.SelectedIndex = -1;
                                        comboTel3.SelectedIndex = -1;
                                    }
                                    if (primerCliente.Telefonos.Count >= 2)
                                    {
                                        PanelIndex = 0;
                                        txt7.Text = primerCliente.Telefonos[1].Numero;
                                        comboTel2.SelectedIndex = 7;
                                        flptxt.Controls.Remove(panel10);
                                        flptxt.Controls.Add(panel7);
                                        flptxt.Controls.Add(panel10);
                                        flptxt.Controls.Remove(panel8);
                                        txt8.Text = "";
                                        comboTel3.SelectedIndex = -1;
                                        btnCancel3.Visible = false;
                                        PanelIndex++;
                                    }
                                    if (primerCliente.Telefonos.Count >= 3)
                                    {
                                        txt8.Text = primerCliente.Telefonos[2].Numero;
                                        comboTel3.SelectedIndex = 7;
                                        flptxt.Controls.Remove(panel10);
                                        flptxt.Controls.Add(panel8);
                                        flptxt.Controls.Add(panel10);
                                        btnCancel1.Visible = false;
                                        btnCancel3.Visible = false;
                                        PanelIndex++;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontraron clientes con el nombre especificado.",
                                        "Cliente no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un ID de cliente o un nombre de cliente" +
                                " junto con sus apelidos para realizar la búsqueda.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    break;
                case "btnModHabit":
                    {
                        // Verifica si se ingresó un código de habitación
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            string codigoHabitacion = txt1.Text.Trim();
                            Habitacion habitacionEncontrada;
                            try
                            {
                                HabitacionBLL habitacionBLL = new HabitacionBLL();
                                // Busca la habitación por su código
                                habitacionEncontrada = habitacionBLL.seleccionarPorId(codigoHabitacion);
                                // Guardamos en habitacionGeneral los valores obtenidos de la busqueda para validar los
                                // datos modificados por el usuario
                                habitacionGeneral = new Habitacion();
                                habitacionGeneral = habitacionEncontrada;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar la habitación: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra la habitación, muestra los detalles
                            if (habitacionEncontrada != null)
                            {
                                //Cambia el nombre del groupbox adaptandolo a la habitación/hotel que se encontró al buscar
                                groupBox2.Text = $"Mobiliario de la Habitación {habitacionEncontrada.CodHabit}";
                                // Rellena los TextBox con la información de la habitación encontrada
                                txt1.Text = habitacionEncontrada.CodHabit;
                                txt2.Text = habitacionEncontrada.CodHotel.ToString();
                                // Selecciona un index del combo dependiendo de la categ de la habit
                                switch (habitacionEncontrada.Categ)
                                {
                                    case "Standard":
                                        combo1.SelectedIndex = 0;
                                        break;
                                    case "Junior":
                                        combo1.SelectedIndex = 1;
                                        break;
                                    case "Premier":
                                        combo1.SelectedIndex = 2;
                                        break;
                                }
                                if (habitacionEncontrada.Soleada)
                                {
                                    combo2.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo2.SelectedIndex = 1;
                                }
                                if (habitacionEncontrada.Lavado)
                                {
                                    combo3.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo3.SelectedIndex = 1;
                                }
                                if (habitacionEncontrada.Nevera)
                                {
                                    combo4.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo4.SelectedIndex = 1;
                                }
                                txt7.Text = habitacionEncontrada.Precio.ToString();
                                switch (habitacionEncontrada.CantPers)
                                {
                                    case 1:
                                        combo5.SelectedIndex = 0;
                                        break;
                                    case 2:
                                        combo5.SelectedIndex = 1;
                                        break;
                                    case 3:
                                        combo5.SelectedIndex = 2;
                                        break;
                                    case 4:
                                        combo5.SelectedIndex = 3;
                                        break;
                                    case 5:
                                        combo5.SelectedIndex = 4;
                                        break;
                                }
                                // obtiene los datos de la list<> habitación encontrada para mostrarlos en el grid view
                                dataGridView1.DataSource = habitacionEncontrada.Mobiliarios;
                                // Cambia el nombre de las column headers
                                dataGridView1.Columns["CodMobil"].HeaderText = "Código Mobilliario";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Código Habitación";
                                dataGridView1.Columns["Descripcion"].HeaderText = "Descripción";
                                dataGridView1.Columns["Precio"].HeaderText = "Precio";
                                // Elimina la columna estado, porque no es necesaria en este caso
                                dataGridView1.Columns["Estado"].Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna habitación con el código especificado.",
                                    "Habitación no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        // Verifica si se ingresó un código de hotel
                        else if (!string.IsNullOrEmpty(txt2.Text))
                        {
                            int codigoHotel;
                            if (int.TryParse(txt2.Text, out codigoHotel))
                            {
                                List<Habitacion> habitacionesEncontradas;
                                try
                                {
                                    HabitacionBLL habitacionBLL = new HabitacionBLL();
                                    // Busca las habitaciones por código de hotel
                                    habitacionesEncontradas = HabitacionBLL.ObtenerHabitacionesPorHotel(codigoHotel);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al buscar las habitaciones: {ex.Message}",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Si se encuentran habitaciones para el hotel, muestra los detalles de la primera habitación
                                if (habitacionesEncontradas != null && habitacionesEncontradas.Count > 0)
                                {
                                    //Cambia el nombre del groupbox adaptandolo a la habitación/hotel que se encontró al buscar
                                    groupBox2.Text = $"Habitaciones del Hotel {codigoHotel}";
                                    // obtiene los datos de la list<> habitación encontrada para mostrarlos en el grid view
                                    dataGridView1.DataSource = habitacionesEncontradas;
                                    // Cambia el nombre de las column headers
                                    dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                    dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                    dataGridView1.Columns["Categ"].HeaderText = "Categoría";
                                    dataGridView1.Columns["CantPers"].HeaderText = "Cant. Pers";
                                    var primeraHabitacion = habitacionesEncontradas[0];

                                    // Guardamos en habitacionGeneral los valores obtenidos de la busqueda para validar los
                                    // datos modificados por el usuario
                                    habitacionGeneral = new Habitacion();
                                    habitacionGeneral = primeraHabitacion;

                                    // Rellena los TextBox/Combos con la información de la primera habitación encontrada
                                    txt1.Text = primeraHabitacion.CodHabit;
                                    switch (primeraHabitacion.Categ)
                                    {
                                        case "Standard":
                                            combo1.SelectedIndex = 0;
                                            break;
                                        case "Junior":
                                            combo1.SelectedIndex = 1;
                                            break;
                                        case "Premier":
                                            combo1.SelectedIndex = 2;
                                            break;
                                    }
                                    if (primeraHabitacion.Soleada)
                                    {
                                        combo2.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo2.SelectedIndex = 1;
                                    }
                                    if (primeraHabitacion.Lavado)
                                    {
                                        combo3.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo3.SelectedIndex = 1;
                                    }
                                    if (primeraHabitacion.Nevera)
                                    {
                                        combo4.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo4.SelectedIndex = 1;
                                    }
                                    txt7.Text = primeraHabitacion.Precio.ToString();
                                    switch (primeraHabitacion.CantPers)
                                    {
                                        case 1:
                                            combo5.SelectedIndex = 0;
                                            break;
                                        case 2:
                                            combo5.SelectedIndex = 1;
                                            break;
                                        case 3:
                                            combo5.SelectedIndex = 2;
                                            break;
                                        case 4:
                                            combo5.SelectedIndex = 3;
                                            break;
                                        case 5:
                                            combo5.SelectedIndex = 4;
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontraron habitaciones para el hotel especificado.",
                                        "Habitaciones no encontradas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El código de hotel debe ser un número válido.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de habitación o un código " +
                                "de hotel para realizar la búsqueda.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                    break;
                case "btnModMobil":
                    {
                        // Verifica si se ingresó un código de mobiliario
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            string codigoMobiliario = txt1.Text.Trim();
                            Mobiliario mobiliarioEncontrado;
                            try
                            {
                                MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();
                                // Busca el mobiliario por su código
                                mobiliarioEncontrado = mobiliarioBLL.SeleccionarPorId(codigoMobiliario);
                                // Guardamos en mobiliarioGeneral los valores obtenidos de la busqueda para validar los
                                // datos modificados por el usuario
                                mobiliarioGeneral = new Mobiliario();
                                mobiliarioGeneral = mobiliarioEncontrado;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar el mobiliario: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra el mobiliario, muestra los detalles
                            if (mobiliarioEncontrado != null)
                            {
                                // Rellena los TextBox con la información del mobiliario encontrado
                                txt1.Text = mobiliarioEncontrado.CodMobil;
                                txt2.Text = mobiliarioEncontrado.CodHabit;
                                txt3.Text = mobiliarioEncontrado.Descripcion;
                                txt4.Text = mobiliarioEncontrado.Precio.ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ningún mobiliario con el código especificado.",
                                    "Mobiliario no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de mobiliario valido para realizar la búsqueda.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    break;
                case "btnModReser":
                    {
                        //Verifica si se ingresó un código de reserva
                        if (!string.IsNullOrEmpty(txt10.Text))
                        {
                            Reservacion Reserva;
                            try
                            {
                                ReservacionBLL reserBLL = new ReservacionBLL();
                                Reserva = reserBLL.SeleccionarPorCodigo(txt10.Text);

                                ReserGeneral = new Reservacion();
                                ReserGeneral = Reserva;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar reserva:{ex.Message}");
                                return;
                            }
                            if (Reserva != null)
                            {
                                txt10.Text = Reserva.CodReserva;
                                txt1.Text = Reserva.IdCliente.ToString();
                                txt2.Text = Reserva.CodHotel.ToString();
                                txt3.Text = Reserva.CodHabit;
                                dateTimePicker1.Value = Reserva.FechaLlegada;
                                dateTimePicker2.Value = Reserva.FechaSalida;
                                switch (Reserva.FormaPago)
                                {
                                    case "Efectivo":
                                        combo6.SelectedIndex = 0;
                                        break;
                                    case "Tarjeta":
                                        combo6.SelectedIndex = 1;
                                        break;
                                    case "Mixto":
                                        combo6.SelectedIndex = 2;
                                        break;

                                }
                                txt7.Text = Reserva.Voucher;
                                txt8.Text = Reserva.MontoTarjeta.ToString();
                                txt9.Text = Reserva.MontoEfectivo.ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna reserva con ese código");
                            }
                        }
                        // Verifica si se ingreso un IdCliente
                        else if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int idCliente;
                            if (int.TryParse(txt1.Text, out idCliente))
                            {
                                try
                                {
                                    //int idCliente = Convert.ToInt32(txt1.Text);
                                    ClienteBLL clienteBLL = new ClienteBLL();

                                    var reservasEncontradas = clienteBLL.VerHistorialReservasPorClienteID(idCliente);

                                    // Si se encuentra la reserva, muestra los detalles
                                    if (reservasEncontradas.Count > 0)
                                    {
                                        dataGridView1.DataSource = reservasEncontradas;
                                        dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                                        dataGridView1.Columns["IdCliente"].HeaderText = "Cédula";
                                        dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                        dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                        dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                                        dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                                        dataGridView1.Columns["FormaPago"].HeaderText = "F. Pago";
                                        dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                                        dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                                        dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                                        dataGridView1.Columns["Estado"].Visible = false;

                                        var primeraReserva = reservasEncontradas[0];

                                        ReserGeneral = new Reservacion();
                                        ReserGeneral = primeraReserva;

                                        // Llenar los TextBox con la información de la reserva
                                        txt10.Text = primeraReserva.CodReserva;
                                        txt1.Text = primeraReserva.IdCliente.ToString();
                                        txt2.Text = primeraReserva.CodHotel.ToString();
                                        txt3.Text = primeraReserva.CodHabit;
                                        dateTimePicker1.Value = primeraReserva.FechaLlegada;
                                        dateTimePicker2.Value = primeraReserva.FechaSalida;
                                        switch (primeraReserva.FormaPago)
                                        {
                                            case "Efectivo":
                                                combo6.SelectedIndex = 0;
                                                break;
                                            case "Tarjeta":
                                                combo6.SelectedIndex = 1;
                                                break;
                                            case "Mixto":
                                                combo6.SelectedIndex = 2;
                                                break;

                                        }
                                        txt7.Text = primeraReserva.Voucher;
                                        txt8.Text = primeraReserva.MontoTarjeta.ToString();
                                        if (primeraReserva.MontoEfectivo > 0)
                                        {
                                            txt9.Text = primeraReserva.MontoEfectivo.ToString();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontró ninguna reserva con el código especificado.", "Reserva no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Debe ingresar un ID de cliente valido para buscar reservas",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        // Verifica si se ingresó un código de habitación
                        else if (!string.IsNullOrEmpty(txt3.Text))
                        {
                            string codHabit = txt3.Text.Trim();
                            DateTime FechaLlegada = dateTimePicker1.Value;
                            DateTime FechaSalida = dateTimePicker2.Value;

                            ReservacionBLL reservacionBLL = new ReservacionBLL();
                            List<Reservacion> reservasHabitacion;
                            try
                            {
                                reservasHabitacion = reservacionBLL.ObtenerReservacionesPorCodHabityFechas(codHabit, FechaLlegada, FechaSalida);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            // Si se encuentra la reserva, muestra los detalles
                            if (reservasHabitacion.Count > 0 /*&& reservasHabitacion != null*/)
                            {
                                dataGridView1.DataSource = reservasHabitacion;
                                dataGridView1.DataSource = reservasHabitacion;
                                dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                                dataGridView1.Columns["IdCliente"].HeaderText = "Cédula";
                                dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                                dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                                dataGridView1.Columns["FormaPago"].HeaderText = "F. Pago";
                                dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                                dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                                dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                                dataGridView1.Columns["Estado"].Visible = false;

                                var primeraReserva = reservasHabitacion[0];

                                ReserGeneral = new Reservacion();
                                ReserGeneral = primeraReserva;

                                // Llenar los TextBox con la información de la reserva
                                txt10.Text = primeraReserva.CodReserva;
                                txt1.Text = primeraReserva.IdCliente.ToString();
                                txt2.Text = primeraReserva.CodHotel.ToString();
                                txt3.Text = primeraReserva.CodHabit;
                                dateTimePicker1.Value = primeraReserva.FechaLlegada;
                                dateTimePicker2.Value = primeraReserva.FechaSalida;
                                switch (primeraReserva.FormaPago)
                                {
                                    case "Efectivo":
                                        combo6.SelectedIndex = 0;
                                        break;
                                    case "Tarjeta":
                                        combo6.SelectedIndex = 1;
                                        break;
                                    case "Mixto":
                                        combo6.SelectedIndex = 2;
                                        break;

                                }
                                txt7.Text = primeraReserva.Voucher;
                                txt8.Text = primeraReserva.MontoTarjeta.ToString();
                                if (primeraReserva.MontoEfectivo > 0)
                                {
                                    txt9.Text = primeraReserva.MontoEfectivo.ToString();
                                }

                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna reserva para la habitación especificada.", "Reserva no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de reserva o un código de habitación para realizar la búsqueda.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    break;
                case "btnModHotel":
                    {
                        // Verifica si se ingreso un codigo de hotel
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int codHotel;
                            if (int.TryParse(txt1.Text, out codHotel))
                            {
                                Hotel hotelEncontrado;
                                try
                                {
                                    HotelBLL hotelBLL = new HotelBLL();
                                    // Busca el hotel por su código
                                    hotelEncontrado = hotelBLL.seleccionarPorId(codHotel);
                                    // Guardamos en hotelGeneral los valores obtenidos de la busqueda para validar los
                                    // datos modificados por el usuario
                                    hotelGeneral = new Hotel();
                                    hotelGeneral = hotelEncontrado;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al buscar el hotel: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Si se encuentra el hotel, muestra los detalles
                                if (hotelEncontrado != null)
                                {
                                    // obtiene los datos de la list<> habitación encontrada para mostrarlos en el grid view  
                                    dataGridView1.DataSource = HabitacionBLL.ObtenerHabitacionesPorHotel(codHotel);
                                    groupBox2.Text = $"Habitaciones del Hotel {codHotel}";
                                    // Cambia el nombre de las column headers
                                    dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                    dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                    dataGridView1.Columns["Categ"].HeaderText = "Categoría";
                                    dataGridView1.Columns["CantPers"].HeaderText = "Cant. Pers";
                                    // Rellena los TextBox con la información del hotel encontrado
                                    txt2.Text = hotelEncontrado.DPais;
                                    txt3.Text = hotelEncontrado.DProvincia;
                                    txt4.Text = hotelEncontrado.DCanton;
                                    txt5.Text = hotelEncontrado.DExacta;

                                    // Rellena los TextBox con los números de teléfono del hotel
                                    if (hotelEncontrado.Telefonos != null)
                                    {
                                        // Asigna los números de teléfono a los TextBox correspondientes

                                        if (hotelEncontrado.Telefonos.Count >= 1)
                                        {
                                            PanelIndex = 0;
                                            btnCancel1.Visible = true;
                                            txt6.Text = hotelEncontrado.Telefonos[0].Numero;
                                            comboTel1.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt7.Text = "";
                                            txt8.Text = "";
                                            comboTel2.SelectedIndex = -1;
                                            comboTel3.SelectedIndex = -1;
                                        }
                                        if (hotelEncontrado.Telefonos.Count >= 2)
                                        {
                                            PanelIndex = 0;
                                            txt7.Text = hotelEncontrado.Telefonos[1].Numero;
                                            comboTel2.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel10);
                                            flptxt.Controls.Add(panel7);
                                            flptxt.Controls.Add(panel10);
                                            flptxt.Controls.Remove(panel8);
                                            txt8.Text = "";
                                            comboTel3.SelectedIndex = -1;
                                            btnCancel3.Visible = false;
                                            PanelIndex++;
                                        }
                                        if (hotelEncontrado.Telefonos.Count >= 3)
                                        {
                                            txt8.Text = hotelEncontrado.Telefonos[2].Numero;
                                            comboTel3.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel10);
                                            flptxt.Controls.Add(panel8);
                                            flptxt.Controls.Add(panel10);
                                            btnCancel1.Visible = false;
                                            btnCancel3.Visible = false;
                                            PanelIndex++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("El hotel encontrado no tiene suficientes números de teléfono.",
                                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontró ningún hotel con el código especificado.",
                                        "Hotel no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El código de hotel debe ser un número válido.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de hotel para realizar la búsqueda.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    break;
                case "btnShowRegis":
                    {
                        // Verifica si se ingresó un ID de cliente
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int idCliente;
                            if (int.TryParse(txt1.Text, out idCliente))
                            {
                                try
                                {
                                    ClienteBLL clienteBLL = new ClienteBLL();
                                    // Busca el cliente por ID
                                    Cliente cliente = clienteBLL.seleccionarPorId(idCliente);
                                    clienteGeneral = new Cliente();
                                    clienteGeneral = cliente;
                                    if (cliente != null)
                                    {
                                        // Rellena los TextBox con la información del cliente encontrado
                                        txt2.Text = cliente.Nombre;
                                        txt3.Text = cliente.Apellido1;
                                        txt4.Text = cliente.Apellido2;
                                        // Asigna los números de teléfono a los TextBox correspondientes                                    
                                        if (cliente.Telefonos.Count >= 1)
                                        {
                                            PanelIndex = 0;
                                            txt5.Text = cliente.Telefonos[0].Numero;
                                            comboTel1.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt7.Text = "";
                                            txt8.Text = "";
                                            comboTel2.SelectedIndex = -1;
                                            comboTel3.SelectedIndex = -1;
                                        }
                                        if (cliente.Telefonos.Count >= 2)
                                        {
                                            PanelIndex = 0;
                                            txt7.Text = cliente.Telefonos[1].Numero;
                                            comboTel2.SelectedIndex = 7;
                                            flptxt.Controls.Add(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt8.Text = "";
                                            comboTel3.SelectedIndex = -1;
                                            PanelIndex++;
                                        }
                                        if (cliente.Telefonos.Count >= 3)
                                        {
                                            txt8.Text = cliente.Telefonos[2].Numero;
                                            comboTel3.SelectedIndex = 7;
                                            flptxt.Controls.Add(panel8);
                                            PanelIndex++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontró a ningún cliente con ese ID",
                                            "NO ENCONTRADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (ArgumentException ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El ID del cliente debe ser un número válido.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        // Verifica si se ingresó el nombre de un cliente
                        else if (!string.IsNullOrEmpty(txt2.Text) && string.IsNullOrEmpty(txt3.Text))
                        {
                            ClienteBLL clienteBLL = new ClienteBLL();
                            // Busca clientes por nombre
                            var clientes = clienteBLL.seleccionarPorNombre(txt2.Text);
                            // Si hay al menos un cliente encontrado, muestra los detalles del primer cliente en la lista
                            if (clientes.Count > 0)
                            {
                                dataGridView1.DataSource = clientes;
                                dataGridView1.Columns["IDCliente"].HeaderText = "Cédula";
                                dataGridView1.Columns["Apellido1"].HeaderText = "Primer Apellido";
                                dataGridView1.Columns["Apellido2"].HeaderText = "Segundo Apellido";
                                dataGridView1.Columns["Activo"].Visible = false;

                                var primerCliente = clientes[0];
                                clienteGeneral = new Cliente();
                                clienteGeneral = primerCliente;
                                txt1.Text = primerCliente.IDCliente.ToString();
                                txt2.Text = primerCliente.Nombre;
                                txt3.Text = primerCliente.Apellido1;
                                txt4.Text = primerCliente.Apellido2;
                                // Asigna los números de teléfono a los TextBox correspondientes
                                if (primerCliente.Telefonos.Count >= 1)
                                {
                                    txt5.Text = primerCliente.Telefonos[0].Numero;
                                    comboTel1.SelectedIndex = 7;
                                    flptxt.Controls.Remove(panel7);
                                    flptxt.Controls.Remove(panel8);
                                }
                                if (primerCliente.Telefonos.Count >= 2)
                                {
                                    txt7.Text = primerCliente.Telefonos[1].Numero;
                                    comboTel2.SelectedIndex = 7;
                                    flptxt.Controls.Add(panel7);
                                    flptxt.Controls.Remove(panel8);
                                    btnCancel1.Visible = false;
                                }
                                if (primerCliente.Telefonos.Count >= 3)
                                {
                                    txt8.Text = primerCliente.Telefonos[2].Numero;
                                    comboTel3.SelectedIndex = 7;
                                    flptxt.Controls.Add(panel8);
                                    btnCancel2.Visible = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron clientes con el nombre especificado.",
                                    "Cliente no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        // Verifica si se ingresó nombre y apellidos de un cliente
                        else if (!string.IsNullOrEmpty(txt2.Text) && !string.IsNullOrEmpty(txt3.Text)
                            && !string.IsNullOrEmpty(txt4.Text))
                        {
                            try
                            {
                                ClienteBLL clienteBLL = new ClienteBLL();
                                // Busca clientes por nombre
                                var clientes = clienteBLL.seleccionarPorNombre(txt2.Text, txt3.Text, txt4.Text);
                                // Si hay al menos un cliente encontrado, muestra los detalles del primer cliente en la lista
                                if (clientes.Count > 0)
                                {
                                    dataGridView1.DataSource = clientes;

                                    dataGridView1.Columns["IDCliente"].HeaderText = "Cédula";
                                    dataGridView1.Columns["Apellido1"].HeaderText = "Primer Apellido";
                                    dataGridView1.Columns["Apellido2"].HeaderText = "Segundo Apellido";
                                    dataGridView1.Columns["Activo"].Visible = false;
                                    var primerCliente = clientes[0];
                                    clienteGeneral = new Cliente();
                                    clienteGeneral = primerCliente;
                                    txt1.Text = primerCliente.IDCliente.ToString();
                                    txt2.Text = primerCliente.Nombre;
                                    txt3.Text = primerCliente.Apellido1;
                                    txt4.Text = primerCliente.Apellido2;
                                    // Asigna los números de teléfono a los TextBox correspondientes
                                    if (primerCliente.Telefonos.Count >= 1)
                                    {
                                        txt5.Text = primerCliente.Telefonos[0].Numero;
                                        comboTel1.SelectedIndex = 7;
                                        flptxt.Controls.Remove(panel7);
                                        flptxt.Controls.Remove(panel8);
                                    }
                                    if (primerCliente.Telefonos.Count >= 2)
                                    {
                                        txt7.Text = primerCliente.Telefonos[1].Numero;
                                        comboTel2.SelectedIndex = 7;
                                        flptxt.Controls.Add(panel7);
                                        flptxt.Controls.Remove(panel8);
                                        btnCancel1.Visible = false;
                                    }
                                    if (primerCliente.Telefonos.Count >= 3)
                                    {
                                        txt8.Text = primerCliente.Telefonos[2].Numero;
                                        comboTel3.SelectedIndex = 7;
                                        flptxt.Controls.Add(panel8);
                                        btnCancel2.Visible = false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontraron clientes con el nombre especificado.",
                                        "Cliente no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un ID de cliente o un nombre de " +
                                "cliente para realizar la búsqueda.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    break;
                case "btnShowHabit":
                    {
                        // Verifica si se ingresó un código de habitación
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            string codigoHabitacion = txt1.Text.Trim();
                            Habitacion habitacionEncontrada;
                            try
                            {
                                HabitacionBLL habitacionBLL = new HabitacionBLL();
                                // Busca la habitación por su código
                                habitacionEncontrada = habitacionBLL.seleccionarPorId(codigoHabitacion);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar la habitación: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra la habitación, muestra los detalles
                            if (habitacionEncontrada != null)
                            {
                                habitacionGeneral = new Habitacion();
                                habitacionGeneral = habitacionEncontrada;

                                // Rellena los TextBox con la información de la habitación encontrada
                                txt1.Text = habitacionEncontrada.CodHabit;
                                txt2.Text = habitacionEncontrada.CodHotel.ToString();
                                switch (habitacionEncontrada.Categ)
                                {
                                    case "Standard":
                                        combo1.SelectedIndex = 0;
                                        break;
                                    case "Junior":
                                        combo1.SelectedIndex = 1;
                                        break;
                                    case "Premier":
                                        combo1.SelectedIndex = 2;
                                        break;
                                }
                                if (habitacionEncontrada.Soleada)
                                {
                                    combo2.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo2.SelectedIndex = 1;
                                }
                                if (habitacionEncontrada.Lavado)
                                {
                                    combo3.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo3.SelectedIndex = 1;
                                }
                                if (habitacionEncontrada.Nevera)
                                {
                                    combo4.SelectedIndex = 0;
                                }
                                else
                                {
                                    combo4.SelectedIndex = 1;
                                }
                                txt7.Text = habitacionEncontrada.Precio.ToString();
                                txt7.Text = ColonFormatter(txt7.Text);
                                switch (habitacionEncontrada.CantPers)
                                {
                                    case 1:
                                        combo5.SelectedIndex = 0;
                                        break;
                                    case 2:
                                        combo5.SelectedIndex = 1;
                                        break;
                                    case 3:
                                        combo5.SelectedIndex = 2;
                                        break;
                                    case 4:
                                        combo5.SelectedIndex = 3;
                                        break;
                                    case 5:
                                        combo5.SelectedIndex = 4;
                                        break;
                                }
                                groupBox2.Text = $"Mobiliario de la Habitación {habitacionEncontrada.CodHabit}";
                                // obtiene los datos de la list<> habitación encontrada para mostrarlos en el grid view
                                dataGridView1.DataSource = habitacionEncontrada.Mobiliarios;

                                // Cambia el nombre de las column headers
                                dataGridView1.Columns["CodMobil"].HeaderText = "Código Mobilliario";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Código Habitación";
                                dataGridView1.Columns["Descripcion"].HeaderText = "Descripción";
                                dataGridView1.Columns["Precio"].HeaderText = "Precio";

                                // Elimina la columna estado, porque no es necesaria en este caso
                                dataGridView1.Columns["Estado"].Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna habitación con el código especificado.",
                                    "Habitación no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        // Verifica si se ingresó un código de hotel
                        else if (!string.IsNullOrEmpty(txt2.Text))
                        {
                            int codigoHotel;
                            if (int.TryParse(txt2.Text, out codigoHotel))
                            {
                                List<Habitacion> habitacionesEncontradas;
                                try
                                {
                                    HabitacionBLL habitacionBLL = new HabitacionBLL();
                                    // Busca las habitaciones por código de hotel
                                    habitacionesEncontradas = HabitacionBLL.ObtenerHabitacionesPorHotel(codigoHotel);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al buscar las habitaciones: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Si se encuentran habitaciones para el hotel, muestra los detalles de la primera habitación
                                if (habitacionesEncontradas != null && habitacionesEncontradas.Count > 0)
                                {
                                    groupBox2.Text = $"Habitaciones del Hotel {codigoHotel}";

                                    dataGridView1.DataSource = habitacionesEncontradas;
                                    dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                    dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                    dataGridView1.Columns["Categ"].HeaderText = "Categoría";
                                    dataGridView1.Columns["CantPers"].HeaderText = "Cant. Pers";

                                    var primeraHabitacion = habitacionesEncontradas[0];

                                    habitacionGeneral = new Habitacion();
                                    habitacionGeneral = primeraHabitacion;

                                    // Rellena los TextBox con la información de la primera habitación encontrada
                                    txt1.Text = primeraHabitacion.CodHabit;
                                    switch (primeraHabitacion.Categ)
                                    {
                                        case "Standard":
                                            combo1.SelectedIndex = 0;
                                            break;
                                        case "Junior":
                                            combo1.SelectedIndex = 1;
                                            break;
                                        case "Premier":
                                            combo1.SelectedIndex = 2;
                                            break;
                                    }
                                    if (primeraHabitacion.Soleada)
                                    {
                                        combo2.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo2.SelectedIndex = 1;
                                    }
                                    if (primeraHabitacion.Lavado)
                                    {
                                        combo3.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo3.SelectedIndex = 1;
                                    }
                                    if (primeraHabitacion.Nevera)
                                    {
                                        combo4.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        combo4.SelectedIndex = 1;
                                    }
                                    txt7.Text = primeraHabitacion.Precio.ToString();
                                    txt7.Text = ColonFormatter(txt7.Text);
                                    switch (primeraHabitacion.CantPers)
                                    {
                                        case 1:
                                            combo5.SelectedIndex = 0;
                                            break;
                                        case 2:
                                            combo5.SelectedIndex = 1;
                                            break;
                                        case 3:
                                            combo5.SelectedIndex = 2;
                                            break;
                                        case 4:
                                            combo5.SelectedIndex = 3;
                                            break;
                                        case 5:
                                            combo5.SelectedIndex = 4;
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontraron habitaciones para el hotel especificado.",
                                        "Habitaciones no encontradas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El código de hotel debe ser un número válido.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de habitación o un " +
                                "código de hotel para realizar la búsqueda.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                    break;
                case "btnShowMobil":
                    {
                        // Verifica si se ingresó un código de mobiliario
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            string codigoMobiliario = txt1.Text.Trim();
                            Mobiliario mobiliarioEncontrado;
                            try
                            {
                                MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();
                                // Busca el mobiliario por su código
                                mobiliarioEncontrado = mobiliarioBLL.SeleccionarPorId(codigoMobiliario);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar el mobiliario: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra el mobiliario, muestra los detalles
                            if (mobiliarioEncontrado != null)
                            {
                                mobiliarioGeneral = new Mobiliario();
                                mobiliarioGeneral = mobiliarioEncontrado;
                                // Rellena los TextBox con la información del mobiliario encontrado
                                txt1.Text = mobiliarioEncontrado.CodMobil;
                                txt2.Text = mobiliarioEncontrado.CodHabit;
                                txt3.Text = mobiliarioEncontrado.Descripcion;
                                txt4.Text = ColonFormatter(mobiliarioEncontrado.Precio.ToString());
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ningún mobiliario con el código especificado.",
                                    "Mobiliario no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        // Verifica si se ingresó un código de habitación
                        else if (!string.IsNullOrEmpty(txt2.Text))
                        {
                            string codigoHabitacion = txt2.Text.Trim();
                            List<Mobiliario> mobiliarioEncontrado;
                            try
                            {
                                MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();
                                // Busca el mobiliario por código de habitación
                                mobiliarioEncontrado = mobiliarioBLL.ObtenerMobiliarioPorHabitacion(codigoHabitacion);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar el mobiliario: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra mobiliario para la habitación, muestra los detalles del primer mobiliario encontrado
                            if (mobiliarioEncontrado != null && mobiliarioEncontrado.Count > 0)
                            {

                                dataGridView1.DataSource = mobiliarioEncontrado;
                                // Cambia el nombre de las column headers
                                dataGridView1.Columns["CodMobil"].HeaderText = "Código Mobilliario";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Código Habitación";
                                dataGridView1.Columns["Descripcion"].HeaderText = "Descripción";
                                dataGridView1.Columns["Precio"].HeaderText = "Precio";
                                // Elimina la columna estado, porque no es necesaria en este caso
                                dataGridView1.Columns["Estado"].Visible = false;

                                var primerMobiliario = mobiliarioEncontrado[0];
                                mobiliarioGeneral = new Mobiliario();
                                mobiliarioGeneral = primerMobiliario;
                                groupBox2.Text = $"Mobiliario de Habitación {primerMobiliario.CodHabit}";
                                // Rellena los TextBox con la información del primer mobiliario encontrado
                                txt1.Text = primerMobiliario.CodMobil;
                                txt2.Text = primerMobiliario.CodHabit;
                                txt3.Text = primerMobiliario.Descripcion;
                                txt4.Text = ColonFormatter(primerMobiliario.Precio.ToString());
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ningún mobiliario para la habitación especificada.",
                                    "Mobiliario no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (!string.IsNullOrEmpty(txt3.Text))
                        {
                            string DescripcionMobiliario = txt3.Text.Trim();
                            string DescripcionBase = DescripcionMobiliario.Split(' ')[0];

                            List<Mobiliario> mobiliarioEncontrado;

                            try
                            {
                                MobiliarioBLL mobiliarioBLL = new MobiliarioBLL();
                                // Busca el mobiliario por código de habitación
                                mobiliarioEncontrado = mobiliarioBLL.seleccionarPorDetalle(DescripcionBase);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar el mobiliario: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Si se encuentra mobiliario para la habitación, muestra los detalles del primer mobiliario encontrado
                            if (mobiliarioEncontrado != null && mobiliarioEncontrado.Count > 0)
                            {
                                groupBox2.Text = $"Mobiliario para la Descripción {DescripcionBase}";
                                // obtiene los datos de la list<> mobiliario encontrado para mostrarlos en el grid view
                                dataGridView1.DataSource = mobiliarioEncontrado;
                                // Cambia el nombre de las column headers
                                dataGridView1.Columns["CodMobil"].HeaderText = "Código Mobilliario";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Código Habitación";
                                dataGridView1.Columns["Descripcion"].HeaderText = "Descripción";
                                dataGridView1.Columns["Precio"].HeaderText = "Precio";
                                // Elimina la columna estado, porque no es necesaria en este caso
                                dataGridView1.Columns["Estado"].Visible = false;

                                var primerMobiliario = mobiliarioEncontrado[0];

                                mobiliarioGeneral = new Mobiliario();
                                mobiliarioGeneral = primerMobiliario;
                                // Rellena los TextBox con la información del primer mobiliario encontrado
                                txt1.Text = primerMobiliario.CodMobil;
                                txt2.Text = primerMobiliario.CodHabit;
                                txt3.Text = primerMobiliario.Descripcion;
                                txt4.Text = ColonFormatter(primerMobiliario.Precio.ToString());
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ningún mobiliario para la descripcion especificada.",
                                    "Mobiliario no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de mobiliario, un código de " +
                                "habitación o una descripción para realizar la búsqueda.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    break;
                case "btnShowReser":
                    {
                        //Verifica si se ingresó un código de reserva
                        if (!string.IsNullOrEmpty(txt10.Text))
                        {
                            Reservacion Reserva;
                            try
                            {
                                ReservacionBLL reserBLL = new ReservacionBLL();
                                Reserva = reserBLL.SeleccionarPorCodigo(txt10.Text);

                                ReserGeneral = new Reservacion();
                                ReserGeneral = Reserva;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al buscar reserva:{ex.Message}");
                                return;
                            }
                            if (Reserva != null)
                            {

                                txt10.Text = Reserva.CodReserva;
                                txt1.Text = Reserva.IdCliente.ToString();
                                txt2.Text = Reserva.CodHotel.ToString();
                                txt3.Text = Reserva.CodHabit;
                                dateTimePicker1.Value = Reserva.FechaLlegada;
                                dateTimePicker2.Value = Reserva.FechaSalida;
                                switch (Reserva.FormaPago)
                                {
                                    case "Efectivo":
                                        combo6.SelectedIndex = 0;
                                        break;
                                    case "Tarjeta":
                                        combo6.SelectedIndex = 1;
                                        break;
                                    case "Mixto":
                                        combo6.SelectedIndex = 2;
                                        break;

                                }
                                txt7.Text = Reserva.Voucher;
                                txt8.Text = ColonFormatter(Reserva.MontoTarjeta.ToString());
                                txt9.Text = ColonFormatter(Reserva.MontoEfectivo.ToString());
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna reserva con ese código");
                            }
                        }
                        // Verifica si se ingreso un IdCliente
                        else if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int idCliente;
                            if (int.TryParse(txt1.Text, out idCliente))
                            {
                                try
                                {
                                    ClienteBLL clienteBLL = new ClienteBLL();

                                    var reservasEncontradas = clienteBLL.VerHistorialReservasPorClienteID(idCliente);

                                    // Si se encuentra la reserva, muestra los detalles
                                    if (reservasEncontradas.Count > 0)
                                    {
                                        dataGridView1.DataSource = reservasEncontradas;
                                        dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                                        dataGridView1.Columns["IdCliente"].HeaderText = "Cédula";
                                        dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                        dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                        dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                                        dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                                        dataGridView1.Columns["FormaPago"].HeaderText = "F. Pago";
                                        dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                                        dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                                        dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                                        dataGridView1.Columns["Estado"].Visible = false;

                                        var primeraReserva = reservasEncontradas[0];

                                        ReserGeneral = new Reservacion();
                                        ReserGeneral = primeraReserva;

                                        // Llenar los TextBox con la información de la reserva
                                        txt10.Text = primeraReserva.CodReserva;
                                        txt1.Text = primeraReserva.IdCliente.ToString();
                                        txt2.Text = primeraReserva.CodHotel.ToString();
                                        txt3.Text = primeraReserva.CodHabit;
                                        dateTimePicker1.Value = primeraReserva.FechaLlegada;
                                        dateTimePicker2.Value = primeraReserva.FechaSalida;
                                        switch (primeraReserva.FormaPago)
                                        {
                                            case "Efectivo":
                                                combo6.SelectedIndex = 0;
                                                break;
                                            case "Tarjeta":
                                                combo6.SelectedIndex = 1;
                                                break;
                                            case "Mixto":
                                                combo6.SelectedIndex = 2;
                                                break;

                                        }
                                        txt7.Text = primeraReserva.Voucher;
                                        txt8.Text = ColonFormatter(primeraReserva.MontoTarjeta.ToString());
                                        if (primeraReserva.MontoEfectivo > 0)
                                        {
                                            txt9.Text = ColonFormatter(primeraReserva.MontoEfectivo.ToString());
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontró ninguna reserva con el código especificado.", "Reserva no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Debe ingresar un ID de cliente valido para buscar reservas",
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        // Verifica si se ingresó un código de hotel
                        else if (!string.IsNullOrEmpty(txt2.Text))
                        {
                            int CodHotel = Convert.ToInt32(txt2.Text.Trim());
                            List<Reservacion> reservasActivas;
                            try
                            {
                                reservasActivas = ReservacionBLL.ReservacionesActivasPorHotel(CodHotel);

                                if (reservasActivas.Count > 0)
                                {
                                    dataGridView1.DataSource = reservasActivas;
                                    dataGridView1.DataSource = reservasActivas;
                                    dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                                    dataGridView1.Columns["IdCliente"].HeaderText = "Cédula";
                                    dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                    dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                    dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                                    dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                                    dataGridView1.Columns["FormaPago"].HeaderText = "F. Pago";
                                    dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                                    dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                                    dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                                    dataGridView1.Columns["Estado"].Visible = false;

                                    var primeraReserva = reservasActivas[0];

                                    ReserGeneral = new Reservacion();
                                    ReserGeneral = primeraReserva;

                                    // Llenar los TextBox con la información de la reserva
                                    txt10.Text = primeraReserva.CodReserva;
                                    txt1.Text = primeraReserva.IdCliente.ToString();
                                    txt2.Text = primeraReserva.CodHotel.ToString();
                                    txt3.Text = primeraReserva.CodHabit;
                                    dateTimePicker1.Value = primeraReserva.FechaLlegada;
                                    dateTimePicker2.Value = primeraReserva.FechaSalida;
                                    switch (primeraReserva.FormaPago)
                                    {
                                        case "Efectivo":
                                            combo6.SelectedIndex = 0;
                                            break;
                                        case "Tarjeta":
                                            combo6.SelectedIndex = 1;
                                            break;
                                        case "Mixto":
                                            combo6.SelectedIndex = 2;
                                            break;

                                    }
                                    txt7.Text = primeraReserva.Voucher;
                                    txt8.Text = ColonFormatter(primeraReserva.MontoTarjeta.ToString());
                                    if (primeraReserva.MontoEfectivo > 0)
                                    {
                                        txt9.Text = ColonFormatter(primeraReserva.MontoEfectivo.ToString());
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontró ninguna reserva para el hotel especificado.", "Reserva no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                        }
                        // Verifica si se ingresó un código de habitación
                        else if (!string.IsNullOrEmpty(txt3.Text))
                        {
                            string codHabit = txt3.Text.Trim();
                            DateTime FechaLlegada = dateTimePicker1.Value;
                            DateTime FechaSalida = dateTimePicker2.Value;

                            ReservacionBLL reservacionBLL = new ReservacionBLL();
                            List<Reservacion> reservasHabitacion;
                            try
                            {
                                reservasHabitacion = reservacionBLL.ObtenerReservacionesPorCodHabityFechas(codHabit, FechaLlegada, FechaSalida);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            // Si se encuentra la reserva, muestra los detalles
                            if (reservasHabitacion.Count > 0 /*&& reservasHabitacion != null*/)
                            {
                                dataGridView1.DataSource = reservasHabitacion;
                                dataGridView1.DataSource = reservasHabitacion;
                                dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                                dataGridView1.Columns["IdCliente"].HeaderText = "Cédula";
                                dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                                dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                                dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                                dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                                dataGridView1.Columns["FormaPago"].HeaderText = "F. Pago";
                                dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                                dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                                dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                                dataGridView1.Columns["Estado"].Visible = false;

                                var primeraReserva = reservasHabitacion[0];

                                ReserGeneral = new Reservacion();
                                ReserGeneral = primeraReserva;

                                // Llenar los TextBox con la información de la reserva
                                txt10.Text = primeraReserva.CodReserva;
                                txt1.Text = primeraReserva.IdCliente.ToString();
                                txt2.Text = primeraReserva.CodHotel.ToString();
                                txt3.Text = primeraReserva.CodHabit;
                                dateTimePicker1.Value = primeraReserva.FechaLlegada;
                                dateTimePicker2.Value = primeraReserva.FechaSalida;
                                switch (primeraReserva.FormaPago)
                                {
                                    case "Efectivo":
                                        combo6.SelectedIndex = 0;
                                        break;
                                    case "Tarjeta":
                                        combo6.SelectedIndex = 1;
                                        break;
                                    case "Mixto":
                                        combo6.SelectedIndex = 2;
                                        break;

                                }
                                txt7.Text = primeraReserva.Voucher;
                                txt8.Text = ColonFormatter(primeraReserva.MontoTarjeta.ToString());
                                if (primeraReserva.MontoEfectivo > 0)
                                {
                                    txt9.Text = ColonFormatter(primeraReserva.MontoEfectivo.ToString());
                                }

                            }
                            else
                            {
                                MessageBox.Show("No se encontró ninguna reserva para la habitación especificada.", "Reserva no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de reserva o un código de habitación para realizar la búsqueda.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    break;
                case "btnShowHotel":
                    {
                        if (!string.IsNullOrEmpty(txt1.Text))
                        {
                            int codHotel;
                            if (int.TryParse(txt1.Text, out codHotel))
                            {
                                Hotel hotelEncontrado;
                                try
                                {
                                    HotelBLL hotelBLL = new HotelBLL();
                                    // Busca el hotel por su código
                                    hotelEncontrado = hotelBLL.seleccionarPorId(codHotel);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al buscar el hotel: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Si se encuentra el hotel, muestra los detalles
                                if (hotelEncontrado != null)
                                {
                                    // Rellena los TextBox con la información del hotel encontrado
                                    hotelGeneral = new Hotel();
                                    hotelGeneral = hotelEncontrado;
                                    txt1.Text = hotelEncontrado.CodHotel.ToString();
                                    txt2.Text = hotelEncontrado.DPais;
                                    txt3.Text = hotelEncontrado.DProvincia;
                                    txt4.Text = hotelEncontrado.DCanton;
                                    txt5.Text = hotelEncontrado.DExacta;

                                    // Rellena los TextBox con los números de teléfono del hotel
                                    if (hotelEncontrado.Telefonos != null)
                                    {
                                        // Asigna los números de teléfono a los TextBox correspondientes                                        
                                        if (hotelEncontrado.Telefonos.Count >= 1)
                                        {
                                            PanelIndex = 0;
                                            txt6.Text = hotelEncontrado.Telefonos[0].Numero;
                                            comboTel1.SelectedIndex = 7;
                                            flptxt.Controls.Remove(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt7.Text = "";
                                            txt8.Text = "";
                                            comboTel2.SelectedIndex = -1;
                                            comboTel3.SelectedIndex = -1;
                                        }
                                        if (hotelEncontrado.Telefonos.Count >= 2)
                                        {
                                            PanelIndex = 0;
                                            txt7.Text = hotelEncontrado.Telefonos[1].Numero;
                                            comboTel2.SelectedIndex = 7;
                                            flptxt.Controls.Add(panel7);
                                            flptxt.Controls.Remove(panel8);
                                            txt8.Text = "";
                                            comboTel3.SelectedIndex = -1;
                                            PanelIndex++;
                                        }
                                        if (hotelEncontrado.Telefonos.Count >= 3)
                                        {
                                            txt8.Text = hotelEncontrado.Telefonos[2].Numero;
                                            comboTel3.SelectedIndex = 7;
                                            flptxt.Controls.Add(panel8);
                                            PanelIndex++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("El hotel encontrado no tiene suficientes números de teléfono.",
                                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontró ningún hotel con el código especificado.",
                                        "Hotel no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El código de hotel debe ser un número válido.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar un código de hotel para realizar la búsqueda.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    break;
                case "btnConta":
                    {
                        int codHotel = 0;
                        DateTime FechaIni = dateTimePicker1.Value;
                        DateTime FechaFin = dateTimePicker2.Value;

                        if (txt3.Text != "" && txt3.Text != null)
                        {
                            codHotel = Convert.ToInt32(txt3.Text.Trim());
                        }

                        if (FechaIni != FechaFin)
                        {
                            var Desglose = ContaBLL.DesglosePagos(FechaIni, FechaFin, codHotel);

                            dataGridView1.DataSource = Desglose.TotalReservaciones;
                            dataGridView1.Columns["CodReserva"].HeaderText = "Código";
                            dataGridView1.Columns["IdCliente"].HeaderText = "Huésped";
                            dataGridView1.Columns["CodHotel"].HeaderText = "Hotel";
                            dataGridView1.Columns["CodHabit"].HeaderText = "Habitación";
                            dataGridView1.Columns["FechaLlegada"].HeaderText = "Llegada";
                            dataGridView1.Columns["FechaSalida"].HeaderText = "Salida";
                            dataGridView1.Columns["FormaPago"].Visible = false;
                            dataGridView1.Columns["Voucher"].HeaderText = "Voucher";
                            dataGridView1.Columns["MontoTarjeta"].HeaderText = "M. Tarjeta";
                            dataGridView1.Columns["MontoEfectivo"].HeaderText = "M. Efectivo";
                            dataGridView1.Columns["Estado"].Visible = false;
                            txt6.Text = ColonFormatter(Desglose.TotalEfectivo.ToString());
                            txt7.Text = ColonFormatter(Desglose.TotalTarjeta.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Debe de agregar un rango de tiempo más extenso",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    break;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnRegHuesped":
                    {
                        txt1.Text = "";
                        txt2.Text = "";
                        txt3.Text = "";
                        txt4.Text = "";
                        txt5.Text = "";
                        txt7.Text = "";
                        txt8.Text = "";
                        comboTel1.SelectedIndex = -1;
                        comboTel2.SelectedIndex = -1;
                        comboTel3.SelectedIndex = -1;

                    }
                    break;
                case "btnRegHabit":
                    {
                        txt1.Text = "";
                        txt2.Text = "";
                        txt7.Text = "";
                        combo1.SelectedIndex = -1;
                        combo2.SelectedIndex = -1;
                        combo3.SelectedIndex = -1;
                        combo4.SelectedIndex = -1;
                        combo5.SelectedIndex = -1;
                    }
                    break;
                case "btnRegMobil":
                    {
                        txt1.Text = "";
                        txt2.Text = "";
                        txt3.Text = "";
                        txt4.Text = "";
                        txt5.Text = "";
                    }
                    break;
                case "btnRegReser":
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
                        txt10.Text = "";
                    }
                    break;
                case "btnRegHotel":
                    {
                        txt1.Text = "";
                        txt2.Text = "";
                        txt3.Text = "";
                        txt4.Text = "";
                        txt5.Text = "";
                        txt6.Text = "";
                        txt7.Text = "";
                        txt8.Text = "";
                        comboTel1.SelectedIndex = -1;
                        comboTel2.SelectedIndex = -1;
                        comboTel3.SelectedIndex = -1;

                    }
                    break;
                case "btnConta":
                    {
                        dateTimePicker1.Value = DateTime.Now;
                        dateTimePicker2.Value = DateTime.Now;
                        txt3.Text = "";
                        txt6.Text = "";
                        txt7.Text = "";
                    }
                    break;
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            switch (BotonPresionado)
            {
                case "btnModRegis":
                    {
                        if (clienteGeneral != null)
                        {
                            txt1.Text = clienteGeneral.IDCliente.ToString();
                            txt2.Text = clienteGeneral.Nombre;
                            txt3.Text = clienteGeneral.Apellido1;
                            txt4.Text = clienteGeneral.Apellido2;
                            if (clienteGeneral.Telefonos.Count >= 1)
                            {
                                PanelIndex = 0;
                                btnCancel1.Visible = true;
                                txt5.Text = clienteGeneral.Telefonos[0].Numero;
                                comboTel1.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel7);
                                flptxt.Controls.Remove(panel8);
                                txt7.Text = "";
                                txt8.Text = "";
                                comboTel2.SelectedIndex = -1;
                                comboTel3.SelectedIndex = -1;
                            }
                            if (clienteGeneral.Telefonos.Count >= 2)
                            {
                                PanelIndex = 0;
                                txt7.Text = clienteGeneral.Telefonos[1].Numero;
                                comboTel2.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel10);
                                flptxt.Controls.Add(panel7);
                                flptxt.Controls.Add(panel10);
                                flptxt.Controls.Remove(panel8);
                                txt8.Text = "";
                                comboTel3.SelectedIndex = -1;
                                btnCancel3.Visible = false;
                                PanelIndex++;
                            }
                            if (clienteGeneral.Telefonos.Count >= 3)
                            {
                                txt8.Text = clienteGeneral.Telefonos[2].Numero;
                                comboTel3.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel10);
                                flptxt.Controls.Add(panel8);
                                flptxt.Controls.Add(panel10);
                                btnCancel1.Visible = false;
                                btnCancel3.Visible = false;
                                PanelIndex++;
                            }
                        }
                    }
                    break;
                case "btnShowRegis":
                    {
                        if (clienteGeneral != null)
                        {
                            txt1.Text = clienteGeneral.IDCliente.ToString();
                            txt2.Text = clienteGeneral.Nombre;
                            txt3.Text = clienteGeneral.Apellido1;
                            txt4.Text = clienteGeneral.Apellido2;
                            if (clienteGeneral.Telefonos.Count >= 1)
                            {
                                PanelIndex = 0;
                                txt5.Text = clienteGeneral.Telefonos[0].Numero;
                                comboTel1.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel7);
                                flptxt.Controls.Remove(panel8);
                                txt7.Text = "";
                                txt8.Text = "";
                                comboTel2.SelectedIndex = -1;
                                comboTel3.SelectedIndex = -1;

                            }
                            if (clienteGeneral.Telefonos.Count >= 2)
                            {
                                PanelIndex = 0;
                                txt7.Text = clienteGeneral.Telefonos[1].Numero;
                                comboTel2.SelectedIndex = 7;
                                flptxt.Controls.Add(panel7);
                                flptxt.Controls.Remove(panel8);
                                txt8.Text = "";
                                comboTel3.SelectedIndex = -1;
                                PanelIndex++;
                            }
                            if (clienteGeneral.Telefonos.Count >= 3)
                            {
                                txt8.Text = clienteGeneral.Telefonos[2].Numero;
                                comboTel3.SelectedIndex = 7;
                                flptxt.Controls.Add(panel8);
                                PanelIndex++;
                            }
                        }
                    }
                    break;
                case "btnModHabit":
                    {
                        if (habitacionGeneral != null)
                        {
                            txt1.Text = habitacionGeneral.CodHabit;
                            txt2.Text = habitacionGeneral.CodHotel.ToString();
                            txt7.Text = habitacionGeneral.Precio.ToString();
                            switch (habitacionGeneral.Categ)
                            {
                                case "Standard":
                                    combo1.SelectedIndex = 0;
                                    break;
                                case "Junior":
                                    combo1.SelectedIndex = 1;
                                    break;
                                case "Premier":
                                    combo1.SelectedIndex = 2;
                                    break;
                            }
                            if (habitacionGeneral.Soleada)
                            {
                                combo2.SelectedIndex = 0;
                            }
                            else
                            {
                                combo2.SelectedIndex = 1;
                            }
                            if (habitacionGeneral.Lavado)
                            {
                                combo3.SelectedIndex = 0;
                            }
                            else
                            {
                                combo3.SelectedIndex = 1;
                            }
                            if (habitacionGeneral.Nevera)
                            {
                                combo4.SelectedIndex = 0;
                            }
                            else
                            {
                                combo4.SelectedIndex = 1;
                            }
                            switch (habitacionGeneral.CantPers)
                            {
                                case 1:
                                    combo5.SelectedIndex = 0;
                                    break;
                                case 2:
                                    combo5.SelectedIndex = 1;
                                    break;
                                case 3:
                                    combo5.SelectedIndex = 2;
                                    break;
                                case 4:
                                    combo5.SelectedIndex = 3;
                                    break;
                                case 5:
                                    combo5.SelectedIndex = 4;
                                    break;
                            }
                        }
                    }
                    break;
                case "btnShowHabit":
                    {
                        if (habitacionGeneral != null)
                        {
                            txt1.Text = habitacionGeneral.CodHabit;
                            txt2.Text = habitacionGeneral.CodHotel.ToString();
                            txt7.Text = ColonFormatter(habitacionGeneral.Precio.ToString());
                            switch (habitacionGeneral.Categ)
                            {
                                case "Standard":
                                    combo1.SelectedIndex = 0;
                                    break;
                                case "Junior":
                                    combo1.SelectedIndex = 1;
                                    break;
                                case "Premier":
                                    combo1.SelectedIndex = 2;
                                    break;
                            }
                            if (habitacionGeneral.Soleada)
                            {
                                combo2.SelectedIndex = 0;
                            }
                            else
                            {
                                combo2.SelectedIndex = 1;
                            }
                            if (habitacionGeneral.Lavado)
                            {
                                combo3.SelectedIndex = 0;
                            }
                            else
                            {
                                combo3.SelectedIndex = 1;
                            }
                            if (habitacionGeneral.Nevera)
                            {
                                combo4.SelectedIndex = 0;
                            }
                            else
                            {
                                combo4.SelectedIndex = 1;
                            }
                            switch (habitacionGeneral.CantPers)
                            {
                                case 1:
                                    combo5.SelectedIndex = 0;
                                    break;
                                case 2:
                                    combo5.SelectedIndex = 1;
                                    break;
                                case 3:
                                    combo5.SelectedIndex = 2;
                                    break;
                                case 4:
                                    combo5.SelectedIndex = 3;
                                    break;
                                case 5:
                                    combo5.SelectedIndex = 4;
                                    break;
                            }
                        }
                    }
                    break;
                case "btnModMobil":
                    {
                        if (mobiliarioGeneral != null)
                        {
                            txt1.Text = mobiliarioGeneral.CodMobil;
                            txt2.Text = mobiliarioGeneral.CodHabit;
                            txt3.Text = mobiliarioGeneral.Descripcion;
                            txt4.Text = mobiliarioGeneral.Precio.ToString();
                        }
                    }
                    break;
                case "btnShowMobil":
                    {
                        if (mobiliarioGeneral != null)
                        {
                            txt1.Text = mobiliarioGeneral.CodMobil;
                            txt2.Text = mobiliarioGeneral.CodHabit;
                            txt3.Text = mobiliarioGeneral.Descripcion;
                            txt4.Text = ColonFormatter(mobiliarioGeneral.Precio.ToString());
                        }
                    }
                    break;
                case "btnModReser":
                    {
                        if (ReserGeneral != null)
                        {
                            txt1.Text = ReserGeneral.IdCliente.ToString();
                            txt2.Text = ReserGeneral.CodHotel.ToString();
                            txt3.Text = ReserGeneral.CodHabit;
                            dateTimePicker1.Value = ReserGeneral.FechaLlegada;
                            dateTimePicker2.Value = ReserGeneral.FechaSalida;
                            switch (ReserGeneral.FormaPago)
                            {
                                case "Efectivo":
                                    combo6.SelectedIndex = 0;
                                    break;
                                case "Tarjeta":
                                    combo6.SelectedIndex = 1;
                                    break;
                                case "Mixto":
                                    combo6.SelectedIndex = 2;
                                    break;
                            }
                            txt7.Text = ReserGeneral.Voucher;
                            txt8.Text = ReserGeneral.MontoTarjeta.ToString();
                            txt9.Text = ReserGeneral.MontoEfectivo.ToString();
                            txt10.Text = ReserGeneral.CodReserva.ToString();
                        }
                    }
                    break;
                case "btnShowReser":
                    {
                        if (ReserGeneral != null)
                        {
                            txt1.Text = ReserGeneral.IdCliente.ToString();
                            txt2.Text = ReserGeneral.CodHotel.ToString();
                            txt3.Text = ReserGeneral.CodHabit;
                            dateTimePicker1.Value = ReserGeneral.FechaLlegada;
                            dateTimePicker2.Value = ReserGeneral.FechaSalida;
                            switch (ReserGeneral.FormaPago)
                            {
                                case "Efectivo":
                                    combo6.SelectedIndex = 0;
                                    break;
                                case "Tarjeta":
                                    combo6.SelectedIndex = 1;
                                    break;
                                case "Mixto":
                                    combo6.SelectedIndex = 2;
                                    break;
                            }
                            txt7.Text = ReserGeneral.Voucher;
                            txt8.Text = ColonFormatter(ReserGeneral.MontoTarjeta.ToString());
                            txt9.Text = ColonFormatter(ReserGeneral.MontoEfectivo.ToString());
                            txt10.Text = ReserGeneral.CodReserva.ToString();
                        }
                    }
                    break;
                case "btnModHotel":
                    {
                        if (hotelGeneral != null)
                        {
                            txt1.Text = hotelGeneral.CodHotel.ToString();
                            txt2.Text = hotelGeneral.DPais;
                            txt3.Text = hotelGeneral.DProvincia;
                            txt4.Text = hotelGeneral.DCanton;
                            txt5.Text = hotelGeneral.DExacta;
                            if (hotelGeneral.Telefonos.Count >= 1)
                            {
                                txt6.Text = hotelGeneral.Telefonos[0].Numero;
                                comboTel1.SelectedIndex = 7;
                            }
                            if (hotelGeneral.Telefonos.Count >= 2)
                            {
                                txt7.Text = hotelGeneral.Telefonos[1].Numero;
                                comboTel2.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel10);
                                flptxt.Controls.Add(panel7);
                                flptxt.Controls.Add(panel10);
                                txt8.Text = "";
                                comboTel3.SelectedIndex = -1;
                                btnCancel3.Visible = true;
                                PanelIndex++;
                            }
                            if (hotelGeneral.Telefonos.Count >= 3)
                            {
                                txt8.Text = hotelGeneral.Telefonos[2].Numero;
                                comboTel3.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel10);
                                flptxt.Controls.Add(panel8);
                                flptxt.Controls.Add(panel10);
                                btnCancel1.Visible = false;
                                btnCancel3.Visible = false;
                                PanelIndex++;
                            }
                        }
                    }
                    break;
                case "btnShowHotel":
                    {
                        if (hotelGeneral != null)
                        {
                            txt1.Text = hotelGeneral.CodHotel.ToString();
                            txt2.Text = hotelGeneral.DPais;
                            txt3.Text = hotelGeneral.DProvincia;
                            txt4.Text = hotelGeneral.DCanton;
                            txt5.Text = hotelGeneral.DExacta;
                            if (hotelGeneral.Telefonos.Count >= 1)
                            {
                                PanelIndex = 0;
                                txt6.Text = hotelGeneral.Telefonos[0].Numero;
                                comboTel1.SelectedIndex = 7;
                                flptxt.Controls.Remove(panel7);
                                flptxt.Controls.Remove(panel8);
                                txt7.Text = "";
                                txt8.Text = "";
                                comboTel2.SelectedIndex = -1;
                                comboTel3.SelectedIndex = -1;
                            }
                            if (hotelGeneral.Telefonos.Count >= 2)
                            {
                                PanelIndex = 0;
                                txt7.Text = hotelGeneral.Telefonos[1].Numero;
                                comboTel2.SelectedIndex = 7;
                                flptxt.Controls.Add(panel7);
                                flptxt.Controls.Remove(panel8);
                                txt8.Text = "";
                                comboTel3.SelectedIndex = -1;
                                PanelIndex++;
                            }
                            if (hotelGeneral.Telefonos.Count >= 3)
                            {
                                txt8.Text = hotelGeneral.Telefonos[2].Numero;
                                comboTel3.SelectedIndex = 7;
                                flptxt.Controls.Add(panel8);
                                PanelIndex++;
                            }
                        }
                    }
                    break;
                case "btnBuscar":
                    {
                        if (clienteGeneral != null)
                        {
                            txt1.Text = clienteGeneral.IDCliente.ToString();
                            txt2.Text = clienteGeneral.Nombre;
                            txt3.Text = clienteGeneral.Apellido1;
                            txt4.Text = clienteGeneral.Apellido2;
                            if (clienteGeneral.Telefonos.Count >= 1)
                            {
                                txt5.Text = clienteGeneral.Telefonos[0].Numero;
                                comboTel1.SelectedIndex = 7;
                            }
                            if (clienteGeneral.Telefonos.Count >= 2)
                            {
                                txt7.Text = clienteGeneral.Telefonos[1].Numero;
                                comboTel2.SelectedIndex = 7;
                            }
                            if (clienteGeneral.Telefonos.Count >= 3)
                            {
                                txt8.Text = clienteGeneral.Telefonos[2].Numero;
                                comboTel3.SelectedIndex = 7;
                            }
                        }
                    }
                    break;
            }
        }

        // Eventos del Formulario CRUD

        private void btnAgregarTel_Click(object sender, EventArgs e)
        {
            if (PanelIndex < paneles.Length)
            {
                if (flptxt.Controls.Contains(panel7))
                {
                    btnCancel1.Visible = false;
                }
                btnCancel3.Visible = false;
                flptxt.Controls.Remove(panel10);
                flptxt.Controls.Add(paneles[PanelIndex]);
                flptxt.Controls.Add(panel10);
                PanelIndex++;
            }
            else
            {
                MessageBox.Show("No se pueden agregar más teléfonos");
            }
        }

        // Botones para quitar los paneles que llevan Telefonos y Borrar los numeros de los textbox

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            flptxt.Controls.Remove(panel7);
            PanelIndex--;
            txt7.Text = "";
            comboTel2.SelectedIndex = -1;
            btnCancel3.Visible = true;
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            flptxt.Controls.Remove(panel8);
            if (flptxt.Controls.Contains(panel7))
            {
                btnCancel1.Visible = true;
            }
            txt8.Text = "";
            comboTel3.SelectedIndex = -1;
            PanelIndex--;
        }

        private void btnCancel3_Click(object sender, EventArgs e)
        {
            if (BotonPresionado == "btnRegHotel" || BotonPresionado == "btnModHotel")
            {
                txt6.Text = "";
                comboTel1.SelectedIndex = -1;
            }
            else
            {
                txt5.Text = "";
                comboTel1.SelectedIndex = -1;
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Verifica si la columna actual es la columna de precio y si el valor de la celda es numérico
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Precio" && e.Value != null &&
                double.TryParse(e.Value.ToString(), out double precio))
            {
                // Formatea el valor de la celda como moneda en la cultura especifica "es-CR" (Español,Colones Costa Rica)
                string precioFormateado = precio.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR"));

                // Asigna el valor formateado a la propiedad FormmatedValue de la celda
                e.Value = precioFormateado;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "MontoTarjeta" && e.Value != null &&
                double.TryParse(e.Value.ToString(), out double MontoTarjeta))
            {
                string precioFormateado = MontoTarjeta.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR"));

                e.Value = precioFormateado;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "MontoEfectivo" && e.Value != null &&
                double.TryParse(e.Value.ToString(), out double MontoEfectivo))
            {
                string precioFormateado = MontoEfectivo.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR"));

                e.Value = precioFormateado;
            }
        }
    }
}
