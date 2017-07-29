<template>
    <Table :columns="columns" :data="data"></Table>
</template>
<script>
	import {HTTP} from '../libs/http-common';
	
    export default {
        data () {
            return {
                columns: [
                    {
                        title: ' ',
                        key: 'avatar',
						render: (h, params) => {
                            return h('img', {
								attrs: {
									src: params.row.avatar,
									width: '48px'
								}
							});
                        }
                    },
					{
						title: '名字',
						key: 'name',
					},
                    {
                        title: '注册时间',
                        key: 'createdDate'
                    },
                    {
                        title: '描述',
                        key: 'description'
                    }
                ],
                data: []
            }
        },
		created() {
			HTTP.get(`/v1/agents/query`)
				.then(response => {
					this.data = response.data.items;
				})
				.catch(e => {
				  this.$Message.error(e);
				})
		}
    }
</script>
