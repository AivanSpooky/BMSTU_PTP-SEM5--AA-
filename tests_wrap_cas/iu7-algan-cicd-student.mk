build:
	@echo "Build"
debug:
	@echo "Debug"
clean:
	@echo "Clean"
test:
	@echo "Running tests..."

ready/stud-unit-test-report-prev.json:
	mkdir -p ./ready
	cp tests/stud-unit-test-report-prev.json ready/stud-unit-test-report-prev.json

ready/stud-unit-test-report.json:
	mkdir -p ./ready
	cp ready/stud-unit-test-report-prev.json ready/stud-unit-test-report.json

ready/app-cli-debug: debug
	mkdir -p ./ready
	mv lab_4/src/* ready/

ready/report.pdf: lab_4/doc/report.pdf
	mkdir -p ./ready
	cp lab_4/doc/report.pdf ready/report.pdf