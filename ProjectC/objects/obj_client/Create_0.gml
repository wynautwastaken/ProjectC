socket = network_create_socket(network_socket_tcp);
var server = network_connect(socket, "127.0.0.1", 3000);

if (server < 0) {
	// error
} else {
	// connected!
}