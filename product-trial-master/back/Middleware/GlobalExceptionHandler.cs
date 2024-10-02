using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

public class GlobalExceptionHandler
{
	private readonly RequestDelegate _next;
	private readonly IHostEnvironment _env;

	public GlobalExceptionHandler(RequestDelegate next, IHostEnvironment env)
	{	_next = next;
		_env = env;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{ await _next(context); }
		catch (Exception ex)
		{ await HandleException(context, ex); }
	}

	private Task HandleException(HttpContext context, Exception ex)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = 500;

		//Si nous somme dans l'environment de développement on affiche le details du message d'erreur.
		//Dans le cas contraire on le laisse vide, l'user n'as pas besoin de voir le détail.
        Console.WriteLine($"Environnement actuel : {_env.EnvironmentName}");

        string details = null;
		if (_env.IsDevelopment())
		{ details = ex.Message; }

		var result = JsonSerializer.Serialize(new
		{	error = "Erreur Interne. Réessayer plus tard...", details = details	});

		return context.Response.WriteAsync(result);
	}
}
