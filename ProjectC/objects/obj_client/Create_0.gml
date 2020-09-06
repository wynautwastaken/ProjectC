socket = network_create_socket(network_socket_tcp);
var server = network_connect(socket, get_string("enter an ip","127.0.0.1"), 7777);

if (server < 0) {
	// error
} else {
	// connected!
}
show_debug_message("Client Connected");