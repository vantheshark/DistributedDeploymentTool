### Run following command for usage:
	
	DD -s LongRunningServiceUniqueId -w 10 --kill ProcessName


### The targe app should implement ISignalListener

### When the target app start, add following line:

	var svc = new Your_Long_Running_Class_Which_Implement_ISignalListener();

	Server.Start<ISignalListener>(svc, "YourServiceName");