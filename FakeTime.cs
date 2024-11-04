using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejoDeTiempos
{
    public class TiempoFalso : Tiempo
    {
        private DateTime tiempo;

        public TiempoFalso()
        {
            //Creamos un objeto que usamos para testear. (4/11/2024)
            tiempo = new DateTime(2024, 11, 4);
        }

        public override DateTime Now()
        {
            return tiempo;
        }

        public void AgregarDias(int cantidad)
        {
            tiempo = tiempo.AddDays(cantidad);
        }

        public void AgregarMinutos(int cantidad)
        {
            tiempo = tiempo.AddMinutes(cantidad);
        }
    }
}