function gridsnap(num) {
	return (num div 8) * 8;
}
function UpdateTilesNear(x, y) {
	if(TileAt(x,y)) {
		with(GetTileInWorld(x,y)) {
			ind = UpdateTile();	
			t_ind = UpdateTileTrans();
		}
	}
	if(TileAt(x-8,y)) {
		with(GetTileInWorld(x-8,y)) {
			ind = UpdateTile();	
			t_ind = UpdateTileTrans();
		}
	}
	if(TileAt(x+8,y)) {
		with(GetTileInWorld(x+8,y)) {
			ind = UpdateTile();	
			t_ind = UpdateTileTrans();
		}
	}
	if(TileAt(x,y+8)) {
		with(GetTileInWorld(x,y+8)) {
			ind = UpdateTile();	
			t_ind = UpdateTileTrans();
		}
	}
	if(TileAt(x,y-8)) {
		with(GetTileInWorld(x,y-8)) {
			ind = UpdateTile();	
			t_ind = UpdateTileTrans();
		}
	}
	transition = false;
	if(sprite_index == spr_tile_stone) {
		if(t_ind != SIDES.ALL) {
			transition = true;
		}
	}
}