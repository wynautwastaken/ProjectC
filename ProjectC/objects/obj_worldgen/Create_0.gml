global.seed = round(power(pi,40));

GenNoise = function(xx,range) {
	var noise = 0;

	var chunkSize = 16;

	range = range div 2;

	while(chunkSize > 0){
	    var chunkIndex = xx div chunkSize;
    
	    var prog = (xx % chunkSize) / chunkSize;
    
	    var left_random = RandomSeed(chunkIndex, range);
	    var right_random = RandomSeed(chunkIndex + 1, range);
    
	    noise += (1-prog)*left_random + prog*right_random;
    
	    chunkSize = chunkSize div 2;
	    range = range div 2;
	    range = max(1,range);
	}

	return noise;
}

RandomSeed = function(seed, range) {
	seed += global.seed;

	random_set_seed(seed);
	rand = irandom_range(0,range);

	return rand;
}

GenWorldStart = function(seed) {
	random_set_seed(seed);
	global.seed = seed;
	i = 0;
	gen_steps = 256;
}
GenWorldPart = function() {
	i++;
	var n = GenNoise(i/4,96);
	for(var j = 0; j < n; j++) {
		var ipos = {x: i*8, y: (96-j)*8};
		var inst = instance_create_layer(gridsnap(ipos.x),gridsnap(ipos.y),layer,obj_tile_1x1);
		inst.tile = types.stone;
		with(inst) event_user(0);
		UpdateTilesNear(gridsnap(ipos.x),gridsnap(ipos.y));
			
		var pos = CalcChunk(i*8,(96-j)*8);
		var posx = pos.x;
		var posy = pos.y;
		var chunkx = pos.chunk_x;
		var chunky = pos.chunk_y;
			
		var grid;
		var chunkid = string(chunkx)+"-"+string(chunky);
		if (ds_map_exists(obj_server.world,chunkid)) {
			grid = ds_map_find_value(obj_server.world,chunkid);
		} else {
			grid = ds_grid_create(256,256);
			ds_map_set(obj_server.world,chunkid,grid);
		}
	
		ds_grid_set(grid,posx,posy,types.stone);
	}
}

GenWorldStart(get_integer("Seed",6942069));
