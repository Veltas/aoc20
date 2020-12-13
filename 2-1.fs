\ Advent of Code 2020 day 2 part 1

decimal

[undefined] stdin [if] s" 2.dat" r/o open-file throw constant stdin [then]

0 value #pad

: aread-line ( c-addr u fid -- n )  read-line if abort" read fail" then drop ;

0 value >pad

: line>pad ( -- )  pad 80 stdin aread-line to #pad  0 to >pad ;

: c-peek ( -- c | -1 )  >pad #pad = if -1 else pad >pad chars + c@ then ;

: c-next ( -- )  >pad 1+ to >pad ;

: c-digit? ( c -- flags )  '0' '9' 1+ within ;

: pass-num ( -- )  begin c-peek c-digit? while c-next repeat ;

: scan-num ( -- str u )  pad >pad chars +  >pad  pass-num  >pad swap - ;

: read-num ( -- n )  scan-num evaluate ;

: read-rule ( -- n1 n2+1 c )
	read-num
	c-next read-num 1+
	c-next c-peek
	c-next c-next ;

: remaining-pad ( -- str u )  pad >pad chars +  #pad >pad - ;

: ccount ( c str u -- n )
	0 rot rot
	chars over + swap ?do
		over i c@ = if 1+ then
	1 chars +loop
	nip ;

: valid-entry? ( -- flags )  read-rule remaining-pad ccount rot rot within ;

: #valid ( -- n )  0 begin line>pad #pad while valid-entry? if 1+ then repeat ;

#valid u. cr
