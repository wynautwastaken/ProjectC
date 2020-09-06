show_debug_message("Client Packet");
var n_id = ds_map_find_value(async_load, "id");

var t = ds_map_find_value(async_load, "type");
if (n_id != socket) {
	
} else {
	if (t == network_type_data) {
		var t_buffer = ds_map_find_value(async_load, "buffer"); 
		var cmd_type = buffer_read(t_buffer, buffer_u8);
		
		switch (cmd_type) {
			case packets.set_block:
				client_handle_setblock(t_buffer);
				break;
		}
	}
}