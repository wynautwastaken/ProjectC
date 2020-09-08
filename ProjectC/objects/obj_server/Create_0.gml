socket_list = ds_list_create();
socket = network_create_server(network_socket_tcp, 7777, 4);
if (socket < 0) {
	// error
}
show_debug_message("Server Created");