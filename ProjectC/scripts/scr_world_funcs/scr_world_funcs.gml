function CalcChunk(world_x, world_y) {
    var r = {};
    var wx = world_x div 256;
    var wy = world_y div 256;
    var cx = wx mod 256;
    var cy = wy mod 8;
    
    r.chunk_x = cx;
    r.chunk_y = cy;
    r.x = world_x mod 256;
    r.y = world_y mod 256;
    
    return r;
}

function CalcWorld(chunk_x, chunk_y, x, y) {
    var r = {};
    var wx = chunk_x * 256;
    var wy = chunk_y * 256;
    
    r.x = wx + x;
    r.y = wy + y;
    
    return r;
}

function TileAt(x,y) {
	return (GetTileInWorld(x,y) > noone);
}

function Tile(type, chunk, cx, cy, xx, yy) constructor {
	Chunk = chunk;
	Cx = cx;
	Cy = cy;
	x = xx;
	y = yy;
	X = x;
	Y = y;
	Type = type;
	
	sprite_index = spr_tile_fresh;
	image_index = 0;
	
	ind = SIDES.NONE;
	tile = types.grass_dirt;

	trans_priority = 0;
	transition = false;
	t_ind = SIDES.ALL;
	other_sprite = spr_tile_dirt;

	static GetWorldPos = function() {
		return CalcWorld(Cx, Cy, x, y);
	}
	static GetChunkPos = function() {
		return {chunk_x: Cx, chunk_y: Cy, x: X, y: Y};	
	}
	
	static GetChunkID = function() {
		return Chunk;	
	}
	
	static GetType = function() {
		return Type;	
	}
	
	static UpdateTileTrans = function() {
		switch(tile) {
			case types.dirt:
				trans_priority = 1;
				break;
			case types.grass_dirt:
				trans_priority = 1;
				break;
			case types.stone:
				trans_priority = 0;
				break;
		}
		var n_ind = SIDES.NONE, left = false, right = false, up = false, down = false;
		if(TileAt(x-8,y)) {
			var inst = GetTileInWorld(x-8,y);
			if(inst.sprite_index != sprite_index) {
				if(inst.trans_priority > trans_priority || (inst.tile == types.grass_dirt && tile == types.dirt)) {
					transition = true;
					left = true;
					other_sprite = inst.sprite_index;
				}
			}
		}
		if(TileAt(x+8,y)) {
			var inst = GetTileInWorld(x+8,y);
			if(inst.sprite_index != sprite_index) {
				if(inst.trans_priority > trans_priority || (inst.tile == types.grass_dirt && tile == types.dirt)) {
					transition = true;
					right = true;
					other_sprite = inst.sprite_index;
				}
			}
		}
		if(TileAt(x,y-8)) {
			var inst = GetTileInWorld(x,y-8);
			if(inst.sprite_index != sprite_index) {
				if(inst.trans_priority > trans_priority) {
					transition = true;
					up = true;
					other_sprite = inst.sprite_index == spr_tile_dirt_grassy? spr_tile_dirt : inst.sprite_index;
				}
			}
		}
		if(TileAt(x,y+8)) {
			var inst = GetTileInWorld(x,y+8);
			if(inst.sprite_index != sprite_index) {
				if(inst.trans_priority > trans_priority) {
					transition = true;
					down = true;
					other_sprite = inst.sprite_index == spr_tile_dirt_grassy? spr_tile_dirt : inst.sprite_index;
				}
			}
		}
	
		if(left && !right && !up && !down) {
			n_ind = SIDES.MIDLEFT; //
		}
		if(left && right && !up && !down) {
			n_ind = SIDES.MIDV;	
		}
		if(left && up && !right && !down) {
			n_ind = SIDES.TOPLEFT;	 //
		}
		if(left && down && !right && !up) {
			n_ind = SIDES.BOTLEFT; //	
		}	
	
		if(right && !left && !up && !down) {
			n_ind = SIDES.MIDRIGHT; //
		}
		if(right && up && !left && !down) {
			n_ind = SIDES.TOPRIGHT; //	
		}
		if(right && down && !left && !up) {
			n_ind = SIDES.BOTRIGHT;	//
		}
	
		if(up && !down && !left && !right) {
			n_ind = SIDES.TOPMID; //	
		}
		if(up && down && !left && !right) {
			n_ind = SIDES.MIDH;
		}
	
		if(down && !up && !left && !right) {
			n_ind = SIDES.BOTMID;
		}
	
		if(left && right && down && !up) {
			n_ind = SIDES.BOT;	
		}
		if(left && right && up && !down) {
			n_ind = SIDES.TOP;
		}
	
		if(up && down && left && !right) {
			n_ind = SIDES.LEFT;
		}
		if(up && down && right && !left) {
			n_ind = SIDES.RIGHT;
		}
	
		if(right && left && up && down) {
			n_ind = SIDES.ALL;	
		}
	
		return n_ind;
	}

	static UpdateTile = function() {
		switch(tile) {
			case types.dirt:
				sprite_index = spr_tile_dirt;
				break;
			case types.grass_dirt:
				sprite_index = spr_tile_dirt_grassy;
				break;
			case types.stone:
				sprite_index = spr_tile_stone;
				break;
		}
	
		var n_ind = SIDES.ALL, left = false, right = false, up = false, down = false;
		if(TileAt(x-8,y)) {
			left = true;
		}
		if(TileAt(x+8,y)) {
			right = true;
		}
		if(TileAt(x,y-8)) {
			up = true;
		}
		if(TileAt(x,y+8)) {
			down = true;
		}
	
		if(left && !right && !up && !down) {
			n_ind = SIDES.RIGHT;
		}
		if(left && right && !up && !down) {
			n_ind = SIDES.MIDH;	
		}
		if(left && up && !right && !down) {
			n_ind = SIDES.BOTRIGHT;	
		}
		if(left && down && !right && !up) {
			n_ind = SIDES.TOPRIGHT;	
		}	
	
		if(right && !left && !up && !down) {
			n_ind = SIDES.LEFT;
		}
		if(right && up && !left && !down) {
			n_ind = SIDES.BOTLEFT;	
		}
		if(right && down && !left && !up) {
			n_ind = SIDES.TOPLEFT;	
		}
	
		if(up && !down && !left && !right) {
			n_ind = SIDES.BOT;	
		}
		if(up && down && !left && !right) {
			n_ind = SIDES.MIDV;
		}
	
		if(down && !up && !left && !right) {
			n_ind = SIDES.TOP;
		}
	
		if(left && right && down && !up) {
			n_ind = SIDES.TOPMID;	
		}
		if(left && right && up && !down) {
			n_ind = SIDES.BOTMID;	
		}
	
		if(up && down && left && !right) {
			n_ind = SIDES.MIDRIGHT;	
		}
		if(up && down && right && !left) {
			n_ind = SIDES.MIDLEFT;	
		}
	
		if(right && left && up && down) {
			n_ind = SIDES.NONE;	
		}
	
		return n_ind;
	}
}

function SetTile(chunkx, chunky, cx, cy, type) {
	var grid;
	var chunkid = GenChunkID(chunkx,chunky);
	if (ds_map_exists(global.world,chunkid)) {
		grid = ds_map_find_value(global.world,chunkid);
	} else {
		grid = ds_grid_create(256,256);
		ds_map_set(global.world,chunkid,grid);
	}
	
	var tile = new Tile(type, chunkid, chunkx, chunky, cx,cy);
	ds_list_add(global.loadedtiles,tile);
	ds_grid_set(grid,cx,cy,tile);	
	var pos = CalcWorld(chunkx,chunky,cx,cy);
	UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
}

function RemoveTile(chunkx, chunky, cx, cy) {
	var grid;
	var chunkid = GenChunkID(chunkx,chunky);
	if (ds_map_exists(global.world,chunkid)) {
		grid = ds_map_find_value(global.world,chunkid);
	} else {
		grid = ds_grid_create(256,256);
		ds_map_set(global.world,chunkid,grid);
	}
	
	var tile = ds_grid_get(grid,cx,cy);
	if(ds_list_find_index(global.loadedtiles,tile) >= 0) {
		ds_list_delete(global.loadedtiles,tile);
	}
	ds_grid_set(grid,cx,cy,noone);	
	var pos = CalcWorld(chunkx,chunky,cx,cy);
	UpdateTilesNear(gridsnap(pos.x),gridsnap(pos.y));
}

function GetTileInChunk(chunkid, cx, cy) {
	var grid;
	if (ds_map_exists(global.world,chunkid)) {
		grid = ds_map_find_value(global.world,chunkid);
	} else {
		grid = ds_grid_create(256,256);
		ds_grid_clear(grid,noone);
		ds_map_set(global.world,chunkid,grid);
	}
	
	return ds_grid_get(grid,cx,cy);	
}

function GetTileInWorld(xx, yy) {
	var pos = CalcChunk(xx,yy);
	var chunkid = GenChunkID(pos.chunk_x,pos.chunk_y);
	return GetTileInChunk(chunkid, pos.x, pos.y);
}

function GenChunkID(chunkx, chunky) {
	return string(chunkx)+"-"+string(chunky);
}

enum SIDES {
	ALL,
	TOPLEFT,
	TOPMID,
	TOPRIGHT,
	TOP,
	MIDLEFT,
	NONE,
	MIDRIGHT,
	MIDV,
	BOTLEFT,
	BOTMID,
	BOTRIGHT,
	BOT,
	LEFT,
	MIDH,
	RIGHT
}

function GetTransition(sprite) {
	switch(sprite) {
		case spr_tile_dirt:
			return spr_tile_dirt_transition_natural;
	}
	return spr_tile_stone_transition_natural;	
}

function GetOverlay(sprite) {
	switch(sprite) {
		case spr_tile_dirt:
			return spr_tile_dirt_overlay;
	}
	return spr_tile_stone_overlay;	
}

function UpdateScreentiles() {
	ds_list_clear(global.screentiles);
	var w = camera_get_view_width(view_camera[0]) div 8;
	var h = camera_get_view_height(view_camera[0]) div 8;
	var _x = camera_get_view_x(view_camera[0]);
	var _y = camera_get_view_y(view_camera[0]);
	for(var i = 0; i < w; i++) {
		for(var j = 0; j < h; j++) {
			var t = GetTileInWorld(gridsnap(i*8) + gridsnap(_x), gridsnap(j*8) + gridsnap(_y));
			if(t != noone) {
				ds_list_add(global.screentiles,t);
			}
		}
	}
}