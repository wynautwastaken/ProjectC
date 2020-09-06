function server_handle_setblock(buffer) {
	buffer_seek(buffer,buffer_seek_start,0);
	var chunkx,chunky,posx,posy, type;
	
	// read data
	type = buffer_read(buffer,buffer_u16);
	chunkx = buffer_read(buffer,buffer_u64);
	chunky = buffer_read(buffer,buffer_u8);
	posx = buffer_read(buffer,buffer_u8);
	posy = buffer_read(buffer,buffer_u8);
	
	// check if chunk exists
	var grid;
	var chunkid = string(chunkx)+"-"+string(chunky);
	if (ds_map_exists(world,chunkid)) {
		grid = ds_map_find_value(world,chunkid);
	} else {
		grid = ds_grid_create(256,256);
		ds_map_add(world,chunkid,grid);
	}
	
	ds_grid_add(grid,posx,posy,type);
	
	// send it back
	for (var i = 0; i < ds_list_size(socket_list); i++) {
		network_send_packet(ds_list_find_value(socket_list,i),buffer,buffer_tell(buffer));
	}
	buffer_delete(buffer);
}