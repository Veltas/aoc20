\ Advent of Code 2020 day 3 part 2

decimal

[undefined] stdin [if] s" 3.dat" r/o open-file throw constant stdin [then]

: read-line! ( c-addr u fid -- n )  read-line throw drop ;

0 value #pad

: line>pad ( -- )  pad 80 stdin read-line! to #pad ;

: len: ( start unit "name" -- )  swap here swap - swap / constant ;

begin-structure path
	field: p.count
	field: p.right
	field: p.down
end-structure

: path, ( right down -- )  0 , swap , , ;

: arr-bound ( arr u unit -- arr-end arr )  * over + swap ;

create paths
	1 1 path,
	3 1 path,
	5 1 path,
	7 1 path,
	1 2 path,

paths path len: #paths

: tree? ( col -- flags )  #pad mod chars pad + c@ '#' = ;

: count-tree! ( n path -- )
	dup p.right @ rot * over p.down @ / tree? if 1 swap +! else drop then ;

: count-tree ( n path -- )
	2dup p.down @ mod 0= if count-tree! else 2drop then ;

: count-paths ( n -- )
	paths #paths path arr-bound do dup i count-tree path +loop drop ;

: count-all ( -- )  0 begin line>pad #pad while dup count-paths 1+ repeat drop ;

: counts. ( -- )
	1 paths #paths path arr-bound do i ? path +loop ;

: count-product ( -- n )
	1 paths #paths path arr-bound do i @ * path +loop ;

count-all count-product . cr
