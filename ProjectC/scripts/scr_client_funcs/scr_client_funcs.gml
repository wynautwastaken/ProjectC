function client_handle_setblock(buffer) {
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
		case types.grass_dirt: obj = obj_tile_1x1; break;
		
		case types.air:
			instance_destroy(instance_position(pos.x,pos.y,obj_tile_1x1));
			UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
			return;
	}
	
	// create the instance
	instance_create_layer(pos.x,pos.y,layer,obj);
	UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
}