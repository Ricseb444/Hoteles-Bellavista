using Hoteles.DAL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;

namespace Hoteles.BLL
{
    public class MobiliarioBLL
    {
        private MobiliarioDAL mobiliarioDAL;

        public MobiliarioBLL()
        {
            mobiliarioDAL = new MobiliarioDAL();
        }

        public ResultadoOperacion Insertar(Mobiliario mobiliario, int cantidad)
        {
            if (mobiliario == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El mobiliario no puede ser null." };
            }

            if (string.IsNullOrWhiteSpace(mobiliario.Descripcion))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La descripción del mobiliario no puede estar vacía." };
            }

            if (mobiliario.Precio < 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El precio del mobiliario no puede ser negativo." };
            }

            if (string.IsNullOrWhiteSpace(mobiliario.CodHabit))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código de habitación no puede estar vacío o solo contener espacios." };
            }

            HabitacionDAL habitacion = new HabitacionDAL();
            if (habitacion.SeleccionarPorId(mobiliario.CodHabit) == null) // Revisa si la habitacion buscada existe
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe la habitación ingresada no existe." };
            }

            try
            {
                mobiliarioDAL.Insertar(mobiliario, cantidad);
                return new ResultadoOperacion { Exito = true, Mensaje = "Mobiliario insertado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al insertar el mobiliario: {ex.Message}" };
            }
        }

        public ResultadoOperacion Actualizar(Mobiliario mobiliario)
        {
            if (mobiliario == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El mobiliario no puede ser null." };
            }

            if (string.IsNullOrWhiteSpace(mobiliario.Descripcion))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La descripción del mobiliario no puede estar vacía." };
            }

            if (mobiliario.Precio < 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El precio del mobiliario no puede ser negativo." };
            }

            if (string.IsNullOrWhiteSpace(mobiliario.CodHabit))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código de habitación no puede estar vacío o solo contener espacios." };
            }

            if (string.IsNullOrWhiteSpace(mobiliario.CodMobil))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código del mobiliario no puede estar vacío o solo contener espacios." };
            }

            Mobiliario existente = MobiliarioDAL.SeleccionarPorId(mobiliario.CodMobil);
            if (existente == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe un mobiliario con el código {mobiliario.CodMobil}." };
            }

            HabitacionDAL habitacion = new HabitacionDAL();
            if (habitacion.SeleccionarPorId(mobiliario.CodHabit) == null) // Revisa si la habitacion buscada existe
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe la habitación ingresada no existe." };
            }

            try
            {
                mobiliarioDAL.Actualizar(mobiliario);
                return new ResultadoOperacion { Exito = true, Mensaje = "Mobiliario actualizado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al actualizar el mobiliario: {ex.Message}" };
            }
        }


        public ResultadoOperacion Eliminar(string codMobil)
        {
            if (string.IsNullOrWhiteSpace(codMobil))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código del mobiliario no puede estar vacío o solo contener espacios." };
            }

            Mobiliario existente = MobiliarioDAL.SeleccionarPorId(codMobil);
            if (existente == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe un mobiliario con el código {codMobil}." };
            }

            try
            {
                mobiliarioDAL.Eliminar(codMobil);
                return new ResultadoOperacion { Exito = true, Mensaje = "Mobiliario eliminado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al eliminar el mobiliario: {ex.Message}" };
            }
        }

        // Método para obtener todo el mobiliario de una habitación específica
        public List<Mobiliario> ObtenerMobiliarioPorHabitacion(string codHabit)
        {
            // Aquí podrías añadir lógica de validación o procesamiento previo si es necesario
            return MobiliarioDAL.SeleccionarMobiliarioPorHabitacionId(codHabit);
        }

        public Mobiliario SeleccionarPorId(string codMobil)
        {
            if (string.IsNullOrWhiteSpace(codMobil) || string.IsNullOrEmpty(codMobil))
            {
                throw new ArgumentException("El código de la habitación está vacio.", nameof(codMobil));
            }

            try
            {
                return MobiliarioDAL.SeleccionarPorId(codMobil);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar la habitación.{ex.Message}", nameof(codMobil));
            }
        }

        public List<Mobiliario> seleccionarPorDetalle(String DetalleMobiliario)
        {
            if (string.IsNullOrWhiteSpace(DetalleMobiliario))
            {
                throw new ArgumentException("El detalle del mobiliario está vacio.", nameof(DetalleMobiliario));
            }

            try
            {
                return mobiliarioDAL.SeleccionarPorDetalle(DetalleMobiliario);
            }
            catch
            {
                throw new ArgumentException("No se pudo seleccionar la lista de Mobiliario por detalle.", nameof(DetalleMobiliario));
            }
        }
    }
}
