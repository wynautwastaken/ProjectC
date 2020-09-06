function client_handle_setblock(buffer) {
	
	buffer_seek(buffer,buffer_seek_start,0);
	//check if from server
	var side = buffer_read(buffer,buffer_bool); 
	show_debug_message("client checking side, side is " + string(side));
	if(side != SERVER_SIDE) return false;
	//discard header
	buffer_read(buffer,buffer_u8); 
	
	// read data
	var posx,posy,chunkx,chunky,type;
	
	type = buffer_read(buffer,buffer_u16);
	chunkx = buffer_read(buffer,buffer_u64);
	chunky = buffer_read(buffer,buffer_u8);
	posx = buffer_read(buffer,buffer_u8)*8;
	posy = buffer_read(buffer,buffer_u8)*8;
	
	var pos = CalcWorld(chunkx,chunky,posx,posy);
	var obj;
	switch (type) {
		default: obj = obj_tile_1x1; break;
		
		case types.air:
			instance_destroy(instance_position(gridsnap(pos.x),gridsnap(pos.y),obj_tile_1x1));
			UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
			return false;
			break;
	}
	
	// create the instance
	instance_create_layer(pos.x,pos.y,layer,obj);
	UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
}