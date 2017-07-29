<template>
	<div>
		<Table :data="items" :columns="columns" stripe></Table>
		<div style="margin: 10px;overflow: hidden">
			<div style="float: right;">
				<Page :total="total" :current="page" @on-change="changePage" show-total></Page>
			</div>
		</div>
	</div>
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
						title: '用户名',
						key: 'userName',
					},
                    {
                        title: '姓名',
                        key: 'fullName'
                    },
                    {
                        title: '邮箱',
                        key: 'email'
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
                items: [],
				total: 0,
				page: 1,
				size: 20,
				changePage () {
					this.created();
				}
            }
        },
		created() {
                HTTP.get(`/v1/account/users`)
				.then(response => {
					this.items = response.data.items;
					this.total = response.data.total;
				})
				.catch(e => {
				  this.$Message.error(e);
				})
		}
    }
</script>