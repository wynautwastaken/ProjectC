// headers are contained in this enum
enum packets {
	set_block,
	/* buffer layout (after header):
	  * u16 - type
	  * u64 - chunk x
	  * u8 - chunk y
	  * u8 - pos x (inside chunk)
	  * u8 - pos y (inside chunk)
	*/
}

enum types {
	air,
	grass_dirt, // obj_tile_1x1
	dirt,
	stone
}