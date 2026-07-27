// Autocomplete.js
const vueSelect2Directive = {
	mounted(el, binding) {
		// 1. Merge defaults with any custom options passed in
		const options = Object.assign({
			placeholder: 'Select...',
			allowClear: true,
			width: '100%'
		}, binding.value || {});

		// 2. Initialize Select2 with the merged options
		$(el).select2(options);

		// 3. Bind events for Vue v-model reactivity
		$(el).on('select2:select select2:unselect', function () {
			el.dispatchEvent(new Event('change', { bubbles: true }));
		});

		// 4. Set initial visual state
		setTimeout(() => {
			$(el).val(el.value).trigger('change.select2');
		}, 0);
	},
	updated(el) {
		$(el).trigger('change.select2');
	},
	unmounted(el) {
		$(el).select2('destroy');
	}
};