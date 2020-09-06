show_debug_message("Server got a Packet");
var n_id = ds_map_find_value(async_load, "id");
var t = ds_map_find_value(async_load, "type");

if (n_id == socket) {
	switch (t) {
		case network_type_connect:
			show_debug_message("Client Connected!");
			ds_list_add(socket_list,ds_map_find_value(async_load,"socket"));
			break;
			
		case network_type_disconnect:
			show_debug_message("Client Disconnected!");
			ds_list_delete(socket_list,ds_list_find_index(socket_list,ds_map_find_value(async_load,"socket")));
			break;
	}
} else {
	if (t == network_type_data) {
			
		var t_buffer = ds_map_find_value(async_load, "buffer");
		buffer_seek(t_buffer,buffer_seek_start,0);
		var side = buffer_read(t_buffer, buffer_bool);
		var cmd_type = buffer_read(t_buffer, buffer_u8);
		//var inst = ds_map_find_value(socket_list, sock);
			
		switch (cmd_type) {
			case packets.set_block:
				server_handle_setblock(t_buffer);
				break;
		}
	}
}