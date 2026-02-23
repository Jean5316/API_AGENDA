using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.ModelViews
{
    public struct Home
    {
        public string Message { get => "API Agenda funcionando!";}
        public string Doc { get => "/swagger";}
    }
}