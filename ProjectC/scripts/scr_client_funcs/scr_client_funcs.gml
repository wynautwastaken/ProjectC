function client_handle_setblock(buffer) {
	
	buffer_seek(buffer,buffer_seek_start,0);
	//check if from server
	var side = buffer_read(buffer,buffer_bool); 
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
	
	switch (type) {
		default:
			break;
		
		case types.air:
			RemoveTile(chunkx,chunky,posx,posy);
			return false;
			break;
	}
	SetTile(chunkx,chunky,posx,posy,type);
}

function client_handle_ping(buffer) {
	
	buffer_seek(buffer,buffer_seek_start,0);
	//check if from server
	var side = buffer_read(buffer,buffer_bool); 
	if(side != SERVER_SIDE) return false;
	ping = string(pingcounter);
	pingcounter = 0;
}