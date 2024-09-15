namespace testBD.services;
using Renci.SshNet;

public class SshTunnelServices
{
    private SshClient? _sshClient;
    private ForwardedPortLocal? _portForwarded;

    public void OpenSshTunnel(string sshHost, int sshPort, string sshUserName, string privateKeyContent, string remouteHost, int remoutePort, string localHost, uint localPort) {
        var privateKey = new PrivateKeyFile(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(privateKeyContent)));
        var keyFiles = new[] { privateKey };
        var methods = new AuthenticationMethod[]
        {
            new PrivateKeyAuthenticationMethod(sshUserName,keyFiles)
        };
        var connectionInfo = new Renci.SshNet.ConnectionInfo(sshHost,sshPort, sshUserName, methods);
        _sshClient = new SshClient(connectionInfo);
        _sshClient.Connect();
        _portForwarded = new ForwardedPortLocal(localHost, localPort, remouteHost, (uint)remoutePort);
        _sshClient.AddForwardedPort(_portForwarded);
        _portForwarded.Start();
    }
    public void CloseSshTunnel() {
        if (_portForwarded != null) {
            _portForwarded.Stop();
            _portForwarded = null;
        }
        if (_sshClient != null) {
            _sshClient.Disconnect();
            _sshClient.Dispose();
            _sshClient = null;
        }
    
}
    public async Task InitializeSshTunnel(IServiceProvider service) {
        var configuration = service.GetService<IConfiguration>();
        string privateKeyFilePath = configuration["Ssh:PrivateKeyFilePath"];
        string keyVaultUrl = configuration["KeyVault:Url"];
        string secretName = configuration["KeyVault:SecretName"];
        string sshHost = configuration["Ssh:Host"];
        int sshPort = int.Parse(configuration["Ssh:Port"]);
        string sshUsername = configuration["Ssh:Username"];
        string remoteHost = configuration["Ssh:RemoteHost"];
        int remotePort = int.Parse(configuration["Ssh:RemotePort"]);
        string localHost = configuration["Ssh:LocalHost"];
        uint localPort = uint.Parse(configuration["Ssh:LocalPort"]);
        string privateKeyContent = await File.ReadAllTextAsync(privateKeyFilePath);

        this.OpenSshTunnel(sshHost, sshPort, sshUsername, privateKeyContent, remoteHost, remotePort, localHost, localPort);
    }
}

