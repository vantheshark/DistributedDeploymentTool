### Run following command for usage:
	
Distributed Deployment tool
Usage: DD [OPTIONS]
    [1] Send exit signal to an application

    [2] Start as a server to listening on remote commands

    [3] Execute a remote commands. Use as a client for [2]

Options:
  -l, --listen, --port=PORT  the PORT on which the deployment service is
                               listenning. This must be an integer.

  -w, --wait=SECONDS         the number of SECONDS to wait.
                               this must be an integer.

  -f, --kill=PROCESS         kill the target PROCESS if timeout.

  -s, --stop=SERVICE         the SERVICE to send stop signal to. This can be
                               anything that unique identify the service

  -r, -e, --execute=COMMAND  execute a COMMAND on remote server

  -t, --target=ADDRESS       the ADDRESS of the remote server in format IP:PORT

  -k, --token, --key=SECURITYTOKEN
                             the SECURITYTOKEN to pass to the remote server

  -h, --help                 show this message and exit



### The targe app should implement ISignalListener

### When the target app start, add following line:

	var svc = new Your_Long_Running_Ap_Which_Implement_ISignalListener();

	Server.Start<ISignalListener>(svc, "YourServiceName");