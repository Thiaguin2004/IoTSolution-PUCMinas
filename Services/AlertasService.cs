using IoTSolution.Data;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class AlertasService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TwilioSettings _twilioSettings;
    private Timer _timer;

    public AlertasService(IServiceProvider serviceProvider, IOptions<TwilioSettings> twilioOptions)
    {
        _serviceProvider = serviceProvider;
        _twilioSettings = twilioOptions.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //_timer = new Timer(ExecutarVerificacao, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private void ExecutarVerificacao(object state)
    {
        // Criar um escopo manualmente para resolver o ApiDbContext
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();  // Resolve o DbContext dentro do escopo

            var leituras = context.Leituras.ToList();
            var sensores = context.Sensors.ToList();

            foreach (var leitura in leituras)
            {
                var sensor = sensores.FirstOrDefault(s => s.IdSensor == leitura.IdSensor);
                if (sensor == null) continue;

                if (leitura.Temperatura < sensor.LimiteInferiorTemperatura || leitura.Temperatura > sensor.LimiteSuperiorTemperatura)
                {
                    if (sensor.EnviouMensagemDesdeUltimaAnomalia && leitura.DataHoraLeitura.AddMinutes(10) <= DateTime.Now)
                    {
                        var dispositivo = context.Dispositivos.FirstOrDefault(d => d.IdDispositivo == leitura.IdDispositivo);
                        if (dispositivo == null) continue;

                        var usuarios = context.Usuarios.Where(u => u.IdUsuario == dispositivo.IdUsuario).ToList();

                        foreach (var usuario in usuarios)
                        {
                            EnviarMensagemTwilio(usuario.Telefone, sensor.Descricao, leitura.Temperatura);
                        }

                        sensor.EnviouMensagemDesdeUltimaAnomalia = true;
                        context.Sensors.Update(sensor);
                        context.SaveChanges();
                    }

                    if (!sensor.EnviouMensagemDesdeUltimaAnomalia)
                    {
                        var dispositivo = context.Dispositivos.FirstOrDefault(d => d.IdDispositivo == leitura.IdDispositivo);
                        if (dispositivo == null) continue;

                        var usuarios = context.Usuarios.Where(u => u.IdUsuario == dispositivo.IdUsuario).ToList();

                        foreach (var usuario in usuarios)
                        {
                            EnviarMensagemTwilio(usuario.Telefone, sensor.Descricao, leitura.Temperatura);
                        }

                        sensor.EnviouMensagemDesdeUltimaAnomalia = true;
                        context.Sensors.Update(sensor);
                        context.SaveChanges();
                    }
                }
            }
        }
    }

    private void EnviarMensagemTwilio(string telefone, string sensorDescricao, decimal temperatura)
    {
        try
        {
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

            var mensagem = MessageResource.Create(
                body: $"Alerta: Temperatura do sensor {sensorDescricao} está fora dos limites! Temperatura atual: {temperatura}°C.",
                from: new Twilio.Types.PhoneNumber(_twilioSettings.FromPhoneNumber),
                to: new Twilio.Types.PhoneNumber("+55" + telefone)
            );
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

public class TwilioSettings
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string FromPhoneNumber { get; set; }
}
