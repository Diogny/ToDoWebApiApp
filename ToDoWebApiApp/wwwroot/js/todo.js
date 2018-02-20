(function () {
	var
		t = $($("div.table>.body").get(0));
	$.get("api/todo")
		.then(e => {
			if (!objIs("Array", e)) {
				return;
			}
			e.forEach(i => {
				t.append($("<div class=\"row clearfix\" item-id=\"" + i.id +
					"\"><div class=\"cell\">" + i.name + "</div><div class=\"cell\">" + i.isComplete + "</div></div>"));
			});
		});
	
	function objIs(type, obj) {
		var clas = Object.prototype.toString.call(obj).slice(8, -1);
		return obj !== undefined && obj !== null && clas === type;
	}
})();
//https://www.ama3.com/anytime/