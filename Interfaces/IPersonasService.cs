using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IPersonasService
    {
        public IEnumerable<PersonaModel> GetAllPersonas();
        public PersonaDireccionModel GetPersonaDireccionByIdPersona(int idPersona);
        public PersonaModel GetPersonaById(int idPersona);
        IEnumerable<PersonaModel> GetAllPersonasMorales(PersonaMoralBusquedaModel model);
    }
}
