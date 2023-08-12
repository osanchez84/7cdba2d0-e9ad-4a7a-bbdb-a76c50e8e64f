﻿using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IPersonasService
    {
        public IEnumerable<PersonaModel> GetAllPersonas();
        List<PersonaModel> BusquedaPersona(PersonaModel model);
        public bool VerificarLicenciaSitteg(string numeroLicencia);
        public void InsertarDesdeServicio(string nombre, string apellidoPaterno, string apellidoMaterno, int tipoLicencia, string numeroLicencia, DateTime fechaExpedicion, DateTime fechaVigencia);
        public PersonaDireccionModel GetPersonaDireccionByIdPersona(int idPersona);
        public PersonaModel GetPersonaById(int idPersona);
        IEnumerable<PersonaModel> GetAllPersonasMorales();
        IEnumerable<PersonaModel> GetAllPersonasMorales(PersonaMoralBusquedaModel model);
        int CreatePersonaMoral(PersonaModel model);
        int UpdatePersonaMoral(PersonaModel model);
        int CreatePersonaDireccion(PersonaDireccionModel model);
        int UpdatePersonaDireccion(PersonaDireccionModel model);
        PersonaModel GetPersonaTypeById(int idPersona);
        public PersonaModel BuscarPersonaSoloLicencia(string numeroLicencia);
        public int UpdatePersona(PersonaModel model);
        public int CreatePersona(PersonaModel model);
    }
}
