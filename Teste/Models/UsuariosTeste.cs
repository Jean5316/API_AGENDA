using API_AGENDA.Models;

namespace Teste.Models;

[TestClass]
public sealed class UsuariosTeste
{
    [TestMethod]
    public void TesteGetSetPropriedades()
    {
        //Arrange
        var usuario = new Usuario();

        //Act
        usuario.Id = 1;
        usuario.Email ="jean@lucas.com";
        usuario.SenhaHash = "Teste";
        usuario.Role = "Admin";
        //Assert
        Assert.AreEqual(1, usuario.Id);
        Assert.AreEqual("jean@lucas.com", usuario.Email);
        Assert.AreEqual("Teste", usuario.SenhaHash);
        Assert.AreEqual("Admin", usuario.Role);


    }
}
