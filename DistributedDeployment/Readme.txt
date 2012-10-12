### Run following command for usage:
	
	DD -s LongRunningService -w 10 --kill


### The targe app should implement ISignalListener

### When the target app start, add following line:

	var svc = new Your_Long_Running_App_Which_Implement_ISignalListener();

	Server.Start<ISignalListener>(svc, "YourServiceName");