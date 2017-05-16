namespace AuthEngine.Engine
{
    /// <summary>
    /// Предоставляет интерфейс для авторизации на сайте
    /// </summary>
    public interface IAuthorizable
    {
        AuthResponse Authorizate(AuthParams prms);
    }
}
