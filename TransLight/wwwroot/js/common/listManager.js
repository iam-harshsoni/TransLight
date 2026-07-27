window.createListManager = function (options) {

    const { ref, watch } = Vue;

    const items = ref([]);
    const pageNumber = ref(1);
    const pageSize = ref(options.pageSize || 10);
    const totalPages = ref(1);
    const totalItems = ref(0);
    const loading = ref(false);

    const search = ref(options.search || {});

    let debounceTimer = null;

    async function load() {

        loading.value = true;

        try {
            const params = new URLSearchParams();

            params.append("pageNumber", pageNumber.value);
            params.append("pageSize", pageSize.value);

            Object.keys(search.value).forEach(key => {

                const value = search.value[key];

                if (value !== null && value !== undefined && value !== "") {
                    params.append(key, value);
                }

            });

            const response = await fetch(`${options.url}?${params}`);

            const data = await response.json();

            items.value = data.items;
            totalItems.value = data.totalItems;
            totalPages.value = data.totalPages;
        }
        finally {
            loading.value = false;
        }
    }

    function changePage(page) {
        if (page < 1 || page > totalPages.value)
            return;
        
        pageNumber.value = page;

        load();

    }

    watch(search, () => {
        clearTimeout(debounceTimer);

        debounceTimer = setTimeout(() => {
            pageNumber.value = 1;
            load();
        }, 400);
    }, { deep: true });

    return {
        items,
        pageNumber,
        pageSize,
        totalPages,
        totalItems,
        loading,
        search,

        load,
        changePage
    };

}