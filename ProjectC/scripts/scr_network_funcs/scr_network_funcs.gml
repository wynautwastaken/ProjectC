
// creates the set block packet
function packet_set_block(side,chunkx,chunky,xx,yy,type) {
	
	var buffer = buffer_create(256,buffer_grow,1);
	buffer_seek(buffer,buffer_seek_start,0);
	
	buffer_write(buffer,buffer_bool,side); // header side
	buffer_write(buffer,buffer_u8,packets.set_block); // header type
	
	buffer_write(buffer,buffer_u16,type); // type
	
	buffer_write(buffer,buffer_u64,chunkx); // chunk x
	buffer_write(buffer,buffer_u8,chunky); // chunk y
	
	buffer_write(buffer,buffer_u8,xx); // x (inside chunk)
	buffer_write(buffer,buffer_u8,yy); // y (inside chunk)
	
	return buffer;
}