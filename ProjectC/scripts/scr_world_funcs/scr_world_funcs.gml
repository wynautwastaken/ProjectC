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