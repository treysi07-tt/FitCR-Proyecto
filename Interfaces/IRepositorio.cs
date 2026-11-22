using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Interfaces
{
    public interface IRepositorio<T> //contrato
    {
        List<T> ObtenerTodos();

        void GuardarTodos(List<T> elementos);
    }
}