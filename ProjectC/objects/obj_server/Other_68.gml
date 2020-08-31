var n_id = ds_map_find_value(async_load, "id");

if (n_id = socket) {
	var t = ds_map_find_value(async_load, "type");
	switch (t) {
		case network_type_connect:
			show_debug_message("Connected!");
			break;
		case network_type_disconnect:
			show_debug_message("Disconnected!");
			break;
		case network_type_data:
			show_debug_message("Data!");
			break;
	}
}