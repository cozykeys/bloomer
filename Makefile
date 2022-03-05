.PHONY: render
render:
	./bloomer.sh render

.PHONY: case
case:
	./bloomer.sh case

.PHONY: ponoko
ponoko:
	./bloomer.sh ponoko

.PHONY: perimeters
perimeters:
	./bloomer.sh perimeters

.PHONY: kicad_pcb
kicad_pcb:
	./bloomer.sh pcb

.PHONY: traces
traces:
	./bloomer.sh traces
